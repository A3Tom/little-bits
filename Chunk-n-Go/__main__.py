from pathlib import Path
import subprocess
import glob
import shutil

target_dir = './a-sound-folder/'
target_file = 'long_ass_sql_file_to_import.sql'
temp_dir = './tmp/'
batch_amount = 1000

target_dbserver = 'localhost'
target_dbname = 'YerMaw'
target_dbuser = 'TychoBrahe'
target_dbpass = 'LegendsNeverDie'
dry_run = True


def getChunkin():
    lastIdx = 0
    statements = []

    with open(f'{target_dir}{target_file}', "r", encoding="utf-8") as file:
        for idx, line in enumerate(file):
            statements.append(line)

            if (idx % batch_amount == 0):
                lastIdx = idx//batch_amount
                buildFileFromStatements(
                    buildFileName(f"{lastIdx:03}"), statements)
                statements = []

        lastIdx+1
        buildFileFromStatements(buildFileName(f"{lastIdx:03}"), statements)


def buildFileFromStatements(filename: str, statements: list):
    with open(f"{temp_dir}{filename}", "w", encoding="utf-8") as file:
        statements = plopInTransactionalStatments(statements)
        for line in statements:
            file.write(line)


def plopInTransactionalStatments(statements: list):
    important_verb = "ROLLBACK" if dry_run else "COMMIT"

    statements.insert(0, f"BEGIN TRANSACTION \n")
    statements.append(f"{important_verb} TRANSACTION")


def buildFileName(prefix: str):
    return f"{prefix}_{target_file}"


def runFile(filename: str):
    cmd = f"SQLCMD -S {target_dbserver} -d {target_dbname} -U {target_dbuser} -P {target_dbpass} -i {filename} -r0 1>None"
    subprocess.Popen(cmd, shell=True, stderr=subprocess.PIPE).stderr.read()


def runFiles():
    files = [f for f in glob.glob(f"{temp_dir}*.sql")]
    for file in files:
        runFile(file)


def createTempDir():
    Path(temp_dir).mkdir(parents=True, exist_ok=True)


def cleanupTempDir():
    shutil.rmtree(temp_dir, ignore_errors=True)


def run():
    cleanupTempDir()
    createTempDir()
    getChunkin()
    runFiles()


if __name__ == '__main__':
    run()
