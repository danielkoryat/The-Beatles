import os
import subprocess
import requests
from unidiff import PatchSet
from dotenv import load_dotenv
import json



openai_models = {
    1: 'gpt-3.5-turbo',           # max 4,096 tokens
    2: 'gpt-3.5-turbo-1106',      # max 16,385 tokens
    3: 'gpt-4',                   # max 8,192 tokens
    4: 'gpt-4-32k',               # max 32,768 tokens
    5: 'gpt-4-1106-preview',      # max 128,000 tokens gpt 4 turbo
}

load_dotenv()
# Load environment variables from .env file
app_type = 'star wars'
gpt_model = openai_models[1]

# Generates a commit message using OpenAI API based on the provided diff
def generate_commit_message(diff):
    api_key = os.getenv('OPENAI_API_KEY')
    endpoint = 'https://api.openai.com/v1/chat/completions'
    headers = {
        'Authorization': f'Bearer {api_key}',
        'Content-Type': 'application/json',
    }
    prompt_content = (
    'Please generate a detailed and well-structured Git commit message for a {app_type} application change. '
    'The commit message should follow these guidelines:\n'
    '- Start with a short, concise subject line, using the imperative mood, for example, "Add", "Update", "Remove". '
    'The subject line should complete the sentence: "If applied, this commit will ..."\n'
    '- Provide a detailed explanatory text in the body'
    'This explanatory text should explain the "what" and "why" behind the changes, not the "how".\n'
    '- Use bullet points to list specific changes. Use a hyphen or asterisk followed by a single space for bullet points, with a hanging indent.\n'
    '- Do not end the subject line with a period. Capitalize the subject line and the beginning of each paragraph.\n'
    '- Include references to any issue tracking identifiers related to this commit at the end of the subject line or in the body.\n\n'
    'Here are the changes that should be described in the commit message:\n\n{diff}\n'
    'Based on these changes, construct the commit message adhering to the guidelines provided. '
    'Remember to contextualize the diff changes into the message, explaining their impact and relevance.'
    'The overall commit message should be under 72 characters at max including the subject line.'
    'Avoid unnecessary repetition, spaces and blank lines in the commit message.'
    'Make the commis message as short as possible and iclude only the relevant changes.'
).format(app_type=app_type, diff=diff)
    payload = {
        'model': gpt_model,
        'messages': [
            {'role': 'system', 'content': prompt_content},
        ],
        'max_tokens': 100,
        'temperature': 0.3
    }
    try:
        response = requests.post(endpoint, json=payload, headers=headers)
        response.raise_for_status()
        commit_message = response.json()['choices'][0]['message']['content'].strip()
        return commit_message
    except requests.exceptions.RequestException as e:
        print(f"an error occurred: {e.response.json()}")
        return None
        

        


def get_staged_diff():
 
        # Get full diff for staged files
        diff_text = subprocess.check_output(['git', 'diff', '--cached'], text=True,encoding='utf-8')
        
        # Parse the diff using unidiff
        patch_set = PatchSet(diff_text)
        diff_data = []

        for patched_file in patch_set:
            # Initialize a dictionary for each file's changes
            file_changes = {
                "file": patched_file.path,
                "changes": []
            }

            for hunk in patched_file:
                for line in hunk:
                    # Include only added or removed lines that don't start with "import"
                    if (line.is_added or line.is_removed) and not line.value.startswith("import"):
                        file_changes["changes"].append({
                            "type": "+" if line.is_added else "-",
                            "content": line.value.strip()
                        })
            
            diff_data.append(file_changes)

        # Convert the diff data to JSON string
        summary_json = json.dumps(diff_data)
        return summary_json
   
    
# Commits changes to the repository with the provided commit message
def commit_changes(commit_message):
    try:
        subprocess.check_output(['git', 'commit', '-m', commit_message])
        print('Changes committed successfully.')
    except subprocess.CalledProcessError as e:
        raise RuntimeError(f'Error committing changes: {e}')

# Main execution
if __name__ == '__main__':
    try:
        diff = get_staged_diff()
        if not diff.strip():
            print('No staged changes to commit.')
            exit(0)
        commit_message = generate_commit_message(diff)
        if not commit_message:
            print('Failed to generate commit message.')
            exit(1)
        print(commit_message)
        #commit_changes(commit_message)
    except RuntimeError as err:
        print(err)
        exit(1)


