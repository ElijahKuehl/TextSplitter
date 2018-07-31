from Tkinter import Tk
from tkFileDialog import askopenfilename


def main():
    sentences = -1
    try:
        # print "Please select a file."
        # Tk().withdraw()
        # file_txt = askopenfilename()
        
        file_txt = raw_input("Please select a file: \n")
        if file_txt == "":
            file_txt = "/Users/ekuehl/Documents/TextSample.txt"
        doc = open(file_txt)
        contents = doc.read()
        contents = contents.replace("Ms.", "Ms/").replace("Mrs.", "Mrs/").replace("Mr.", "Mr/")
    except Exception:
        print "File is unreadable or does not exist. Please try again.\n"
        main()

    contents = contents.replace(".", "*").replace("?", "*").replace("!", "*")
    splitter = contents.split("*")  # .split("?").split("!")  #.replace(":", "*:")#.replace("*", ":")
    for sentence in splitter:
        sentences += 1
        sentence = sentence.replace("\n", "").replace("\r", " ").replace("Ms/", "Ms.").replace("Mrs/", "Mrs.").replace("Mr/", "Mr.")
        sentence_list = list(sentence)
        try:
            while sentence_list[0] == " ":
                del sentence_list[0]
                sentence = "".join(sentence_list)
        except Exception:
            pass
        print sentence

    print "There are", sentences, "sentences in this passage.\n"
    search(contents)


def search(contents):
    word_num = 0
    print "What word would you like to look for? Type !q to quit."
    word = raw_input("")
    if word == "!q":
        return
    try:
        split_text = contents.split(word)
    except Exception:
        print "Error. Cannot have a blank input. Please try again.\n"
        search(contents)
        return

    for i in split_text:
        word_num += 1
    word_num -= 1

    print "\nThere is", word_num, word, "in this passage.\n"
    search(contents)

main()
