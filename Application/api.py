from flask import Flask, abort, jsonify, request
import json
import torch
from transformers import PegasusForConditionalGeneration, PegasusTokenizer

path = "Model"
torch_device = "cuda" if torch.cuda.is_available() else "cpu"
tokenizer = PegasusTokenizer.from_pretrained(path)
model = PegasusForConditionalGeneration.from_pretrained(path).to(torch_device)

app = Flask(__name__)


@app.route("/api/create", methods=["GET"])
def text_preparattion(text):
    sentences = text.split(".")

    sent_paraph_map = {}

    for sent in sentences:
        if sent:
            predictions = paraphrase_text(sent)
            sent_paraph_map[sent] = predictions

    output = []

    for i in range(5):
        curr_sentences = []
        for sent in sentences:
            if sent in sent_paraph_map:
                curr_sentences.append(sent_paraph_map[sent][i])

        paragraph = " ".join(curr_sentences)
        output.append(paragraph)

    return output


def paraphrase_text(text, num_sentences=5):
    '''Generate text paraphrases'''
    batch = tokenizer.prepare_seq2seq_batch(
        [text], truncation=True, padding="longest", max_length=60, return_tensors="pt"
    ).to(torch_device)
    translated = model.generate(
        **batch,
        max_length=60,
        num_beams=10,
        num_return_sequences=num_sentences,
        temperature=1.5
    )
    preds = tokenizer.batch_decode(translated, skip_special_tokens=True)

    return preds


@app.route("/api/predict", methods=["POST"])
def predict():
    'Generate Predictions'
    data = request.get_json(force=True)
    predictions = text_preparattion(data["text"])
    results = {}
    results["response"] = predictions
    return json.dumps(results)


if __name__ == "__main__":
    app.run(port=8000, debug=True)
