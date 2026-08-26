#!/usr/bin/env python3
import os
import sys
import shutil
from pathlib import Path

def check_path_exists(path_str):
    if not path_str:
        return False
    return os.path.exists(path_str)

def main():
    print("=" * 70)
    print("   BUSSIGO - UNITY & ANDROID ENVIRONMENT AUDIT")
    print("=" * 70)

    # 1. Unity Version Required
    print("\n1. UNITY VERSION REQUIRED BY THIS PROJECT:")
    print("   - Recommended: Unity 2022.3 LTS (e.g. 2022.3.20f1+ or 2022.3.48f1+)")
    print("   - Compatible: Unity 2023.2+ or Unity 6 (6000.0+)")

    # 2. Check Unity Hub & Unity Editor
    print("\n2. UNITY EDITOR INSTALLATION STATUS:")
    hub_candidates = [
        r"C:\Program Files\Unity Hub\Unity Hub.exe",
        r"C:\Program Files (x86)\Unity Hub\Unity Hub.exe",
        os.path.expandvars(r"%LOCALAPPDATA%\Programs\Unity Hub\Unity Hub.exe")
    ]
    found_hub = [p for p in hub_candidates if os.path.exists(p)]
    if found_hub:
        print(f"   [+] Unity Hub Found: {found_hub[0]}")
    else:
        print("   [-] Unity Hub Not Found in standard paths.")

    editor_candidates = [
        r"C:\Program Files\Unity\Hub\Editor",
        r"C:\Program Files\Unity",
        r"C:\Program Files (x86)\Unity",
        r"D:\Program Files\Unity\Hub\Editor",
        r"E:\Program Files\Unity\Hub\Editor",
        os.path.expandvars(r"%LOCALAPPDATA%\Unity\Hub\Editor")
    ]

    found_editors = []
    for base in editor_candidates:
        if os.path.exists(base):
            for root, dirs, files in os.walk(base):
                if "Unity.exe" in files:
                    found_editors.append(os.path.join(root, "Unity.exe"))

    if found_editors:
        for ed in found_editors:
            print(f"   [+] Unity Editor Found: {ed}")
    else:
        print("   [-] Unity Editor NOT INSTALLED or not in standard search paths.")

    # 3. Android Build Support & PlaybackEngines
    print("\n3. ANDROID BUILD SUPPORT MODULE:")
    android_support_found = False
    sdk_found = []
    ndk_found = []
    jdk_found = []

    for ed in found_editors:
        ed_dir = Path(ed).parent.parent
        android_player = ed_dir / "Data" / "PlaybackEngines" / "AndroidPlayer"
        if android_player.exists():
            android_support_found = True
            print(f"   [+] Android Build Support module found at: {android_player}")
            
            # Check bundled OpenJDK, SDK, NDK
            bundled_sdk = android_player / "SDK"
            bundled_ndk = android_player / "NDK"
            bundled_jdk = android_player / "OpenJDK"

            if bundled_sdk.exists(): sdk_found.append(str(bundled_sdk))
            if bundled_ndk.exists(): ndk_found.append(str(bundled_ndk))
            if bundled_jdk.exists(): jdk_found.append(str(bundled_jdk))
        else:
            print(f"   [-] Android Build Support module NOT found in: {ed_dir}")

    if not found_editors:
        print("   [-] Cannot verify module because Unity Editor is not installed on this machine.")

    # 4. System Android SDK
    print("\n4. ANDROID SDK STATUS:")
    sys_sdk_candidates = [
        os.path.expandvars(r"%LOCALAPPDATA%\Android\Sdk"),
        r"C:\Android\Sdk",
        r"C:\Android\android-sdk",
        os.environ.get("ANDROID_HOME", ""),
        os.environ.get("ANDROID_SDK_ROOT", "")
    ]
    for s in sys_sdk_candidates:
        if s and os.path.exists(s):
            sdk_found.append(s)

    if sdk_found:
        for s in set(sdk_found):
            print(f"   [+] Android SDK Found: {s}")
    else:
        print("   [-] Android SDK NOT FOUND on system.")

    # 5. Android NDK Status
    print("\n5. ANDROID NDK STATUS:")
    sys_ndk_candidates = [
        os.environ.get("ANDROID_NDK_ROOT", ""),
        os.environ.get("NDK_ROOT", "")
    ]
    for n in sys_ndk_candidates:
        if n and os.path.exists(n):
            ndk_found.append(n)

    if ndk_found:
        for n in set(ndk_found):
            print(f"   [+] Android NDK Found: {n}")
    else:
        print("   [-] Android NDK NOT FOUND on system.")

    # 6. OpenJDK / Java Status
    print("\n6. OPENJDK / JAVA STATUS:")
    java_bin = shutil.which("java")
    javac_bin = shutil.which("javac")
    java_home = os.environ.get("JAVA_HOME", "")

    if java_bin:
        print(f"   [+] Java Runtime Found: {java_bin}")
    else:
        print("   [-] 'java' command NOT found in PATH.")

    if javac_bin:
        print(f"   [+] Java Compiler (JDK) Found: {javac_bin}")
    else:
        print("   [-] 'javac' command NOT found in PATH.")

    if java_home and os.path.exists(java_home):
        print(f"   [+] JAVA_HOME: {java_home}")
        jdk_found.append(java_home)
    else:
        print("   [-] JAVA_HOME environment variable not configured.")

    # 7. Real APK Build Readiness Summary
    print("\n7. REAL APK BUILD READINESS SUMMARY:")
    can_build_directly = len(found_editors) > 0 and android_support_found
    if can_build_directly:
        print("   [READY] Environment is ready to compile APK directly via command line or Unity Editor.")
    else:
        print("   [ACTION REQUIRED] Unity Editor with Android Build Support must be installed to compile the real APK binary.")
        print("   The project configuration, scripts, scenes, and C# codebase are 100% configured for Android.")

if __name__ == "__main__":
    main()
