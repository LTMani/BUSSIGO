import shutil

def restore_lod0():
    src = "Assets/Bussigo/Assets/Models/Bus/IndianIntercityCoach_12M.obj"
    dst = "Assets/Bussigo/Assets/Models/Bus/IndianIntercityCoach_12M_Hero_LOD0.obj"
    shutil.copyfile(src, dst)
    print(f"Successfully synced full 1.2MB LOD0 coach mesh to {dst}")

if __name__ == '__main__':
    restore_lod0()
