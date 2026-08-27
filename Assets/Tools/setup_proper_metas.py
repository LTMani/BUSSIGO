import os
from pathlib import Path

def setup_proper_metas():
    # 1. Textures
    textures = {
        "Coach_Livery_Albedo_2K.png": ("de67584467e0a534c9ae88e0ee1cd64b", 0, False),  # Default 2D
        "Coach_Livery_Normal_2K.png": ("3e8acfcfca6410b4397822d57bbac204", 1, False),  # Normal Map 2D
        "Coach_Livery_Roughness_2K.png": ("aa166ae1a4775b241b15f1eab88aa537", 0, False), # Default 2D Linear
        "Cockpit_Dashboard_Cluster_2K.png": ("2096ccbf423f22e468ebcf4eaae032b0", 0, False), # Default 2D
        "Seating_Velvet_Albedo_2K.png": ("d19f942452bdfea41ba6bae2e29c9678", 0, False), # Default 2D
        "Wheel_Rim_Tire_2K.png": ("207d3be17be9a9f4d97f31b8a2aab8f3", 0, False), # Default 2D
    }

    tex_dir = Path("Assets/Bussigo/Assets/Textures")
    for filename, (guid, tex_type, is_alpha) in textures.items():
        meta_path = tex_dir / f"{filename}.meta"
        srgb = 0 if 'normal' in filename.lower() or 'roughness' in filename.lower() else 1
        linear = 1 if 'normal' in filename.lower() or 'roughness' in filename.lower() else 0
        content = f"""fileFormatVersion: 2
guid: {guid}
TextureImporter:
  internalIDToNameTable: []
  externalObjects: {{}}
  serializedVersion: 13
  mipmaps:
    mipMapMode: 0
    enableMipMap: 1
    sRGBTexture: {srgb}
    linearTexture: {linear}
    fadeOut: 0
    borderMipMap: 0
    mipMapsPreserveCoverage: 0
    alphaTestReferenceValue: 0.5
    mipMapFadeDistanceStart: 1
    mipMapFadeDistanceEnd: 3
  bumpmap:
    convertToNormalMap: 0
    externalNormalMap: 0
    heightScale: 0.25
    normalMapFilter: 0
  isReadable: 1
  streamingMipmaps: 0
  streamingMipmapsPriority: 0
  vTOnly: 0
  ignoreMasterTextureLimit: 0
  grayScaleToAlpha: 0
  generateCubemap: 0
  cubemapConvolution: 0
  seamlessCubemap: 0
  textureFormat: 1
  maxTextureSize: 2048
  textureSettings:
    serializedVersion: 2
    filterMode: 1
    aniso: 1
    mipBias: 0
    wrapU: 0
    wrapV: 0
    wrapW: 0
  nPOTScale: 1
  lightmap: 0
  compressionQuality: 50
  spriteMode: 0
  spriteExtrude: 1
  spriteMeshType: 1
  alignment: 0
  spritePivot: {{x: 0.5, y: 0.5}}
  spritePixelsToUnits: 100
  spriteBorder: {{x: 0, y: 0, z: 0, w: 0}}
  spriteGenerateFallbackPhysicsShape: 1
  alphaUsage: 1
  alphaIsTransparency: 0
  spriteTessellationDetail: -1
  textureType: {tex_type}
  textureShape: 1
  singleChannelComponent: 0
  flipbook: 0
  ignorePngGamma: 0
  cookieLightType: 0
  platformSettings:
  - serializedVersion: 3
    buildTarget: DefaultTexturePlatform
    maxTextureSize: 2048
    resizeAlgorithm: 0
    textureFormat: -1
    textureCompression: 1
    compressionQuality: 50
    crunchedCompression: 0
    allowsAlphaSplitting: 0
    overridden: 0
    ignorePlatformSupport: 0
    androidETC2FallbackOverride: 0
    forceMaximumCompressionQuality_BC6H_BC7: 0
"""
        with open(meta_path, "w", encoding="utf-8") as f:
            f.write(content)
        print(f"Updated TextureImporter meta: {meta_path} (textureShape: 1 [Texture2D], generateCubemap: 0)")

    # 2. Models
    models = {
        "IndianIntercityCoach_12M_Hero_LOD0.obj": "430aad4d96f51964d93b8d6e5b26aa4a",
        "IndianIntercityCoach_12M.obj": "b29a2a6250614b54c8ce9c13bc5f87e8",
        "IndianIntercityCoach_12M_Hero_LOD1.obj": "d1c9a6821f9389d4ca1475e1fa90d039",
        "IndianIntercityCoach_12M_Hero_LOD2.obj": "c86fa26dddc0faa49beb2741df32ca10",
        "IndianIntercityCoach_12M_Blockout.obj": "51ef5f5dd8502a141bf73ea5c0df1f83"
    }

    model_dir = Path("Assets/Bussigo/Assets/Models/Bus")
    for filename, guid in models.items():
        meta_path = model_dir / f"{filename}.meta"
        content = f"""fileFormatVersion: 2
guid: {guid}
ModelImporter:
  serializedVersion: 23
  internalIDToNameTable: []
  externalObjects: {{}}
  materials:
    materialImportMode: 0
    materialName: 0
    materialSearch: 1
    materialLocation: 1
  animations:
    legacyGenerateAnimations: 4
    bakeSimulation: 0
    resampleCurves: 1
    animationType: 0
    importAnimation: 0
  meshes:
    lODScreenPercentages: []
    globalScale: 1
    useFileUnits: 1
    keepQuads: 0
    indexFormat: 1
    weldVertices: 1
    bakeAxisConversion: 0
    importBlendShapes: 1
    importCameras: 0
    importLights: 0
    nodeNameCollisionStrategy: 0
    generateColliders: 0
    useSRGBMaterialColor: 1
    sortHierarchyByName: 1
"""
        with open(meta_path, "w", encoding="utf-8") as f:
            f.write(content)
        print(f"Updated ModelImporter meta: {meta_path}")

if __name__ == '__main__':
    setup_proper_metas()
