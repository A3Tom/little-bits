# Chunk n Go

That's right. I've given this useful hing a totally shite name, n wit.  

A small python script that'll chunk a file of any size into smaller, more managable files and fire them straight intae SQLCMD aimed at a server of yer choice  

I wrote this when dealing with a total shanner sql server that couldn't deal with me trying to import batches of 50k lines manually and there was absolutely nae chance I was chunkin up 1M+ rows manually per table when this glorious swiss-army knife of a language is so fun to write

## Usage

Dead simple - probably should make it less simple and paramaterize it but we are where we are

`py __main__.py`

Set yer variables in the file before hand & off you go