import spacy
import json
import os

name = []
nlp = spacy.load("en_core_web_sm")


file = open('sentenceName.txt', 'r')
namer = file.readlines()
file.close()

def startName():
    nameWord = []
    nameText = " ".join(namer)
    doc = nlp(nameText.title()+" And")
    for token in doc:
      if token.pos_ in ["PROPN"]:
        nameWord.append(token.text)
    name = " ".join(nameWord)
    return name

file = open("nameDoc.txt", 'w')
file.write(startName())