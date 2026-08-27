def check_active_input():
    with open("ProjectSettings/ProjectSettings.asset", "r", encoding="utf-8", errors="ignore") as f:
        for line in f:
            if "activeInputHandler" in line:
                print(line.strip())

if __name__ == '__main__':
    check_active_input()
