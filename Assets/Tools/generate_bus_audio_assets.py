#!/usr/bin/env python3
"""
BUSSIGO - Phase 4 Multi-Layer Acoustic Audio Sample Generator
Authors 13 genuine 44.1kHz 16-bit PCM .WAV audio assets:
Engine (Idle, Low, Mid, High RPM), Turbo, Transmission, Air Brakes, Compressor,
Retarder, Doors, Indicators, Musical Air Horn, and Tire/Road Hum.
"""

import math
import struct
import wave
from pathlib import Path
import random

SAMPLE_RATE = 44100

def write_wav(filename, samples, channels=1):
    out_dir = Path(r"T:\Git Project\BUSSIGO\Assets\Bussigo\Assets\Audio")
    out_dir.mkdir(parents=True, exist_ok=True)
    filepath = out_dir / filename
    
    with wave.open(str(filepath), "wb") as wf:
        wf.setnchannels(channels)
        wf.setsampwidth(2) # 16-bit PCM
        wf.setframerate(SAMPLE_RATE)
        
        # Pack to 16-bit signed integer
        data = bytearray()
        for s in samples:
            clamped = max(-1.0, min(1.0, s))
            val = int(clamped * 32767.0)
            data.extend(struct.pack("<h", val))
        wf.writeframes(data)
        
    dur = len(samples) / (SAMPLE_RATE * channels)
    print(f"Generated WAV Asset: {filename} ({dur:.2f}s, {channels}ch, 44.1kHz)")
    return filepath

def generate_engine_layer(filename, duration, base_hz, cylinder_count=6):
    num_samples = int(duration * SAMPLE_RATE)
    samples = []
    
    # 6-cylinder 4-stroke firing frequency: (RPM / 60) * (6 / 2) = base_hz * 3
    firing_hz = base_hz * (cylinder_count / 2.0)
    
    for i in range(num_samples):
        t = i / SAMPLE_RATE
        
        # Fundamental cylinder compression thrum (Sub-bass sine)
        sub = 0.55 * math.sin(2.0 * math.pi * firing_hz * t)
        
        # 2nd & 3rd engine block harmonics (Triangle / Warm sine)
        h2 = 0.25 * math.sin(2.0 * math.pi * firing_hz * 2.0 * t + 0.3)
        h3 = 0.12 * math.sin(2.0 * math.pi * firing_hz * 3.0 * t + 0.7)
        
        # Exhaust manifold low-frequency puff (Filtered pulse)
        pulse = 0.15 * math.sin(2.0 * math.pi * firing_hz * 0.5 * t) ** 3
        
        # Mechanical valvetrain chatter (High-frequency band-limited noise)
        chatter = 0.03 * (random.random() * 2.0 - 1.0) * math.sin(2.0 * math.pi * firing_hz * 4.0 * t)
        
        # Smooth loop envelope crossfade at start/end
        fade = 1.0
        if i < 2000:
            fade = i / 2000.0
        elif i > num_samples - 2000:
            fade = (num_samples - i) / 2000.0
            
        sample = (sub + h2 + h3 + pulse + chatter) * fade * 0.75
        samples.append(sample)
        
    return write_wav(filename, samples, channels=1)

def generate_turbo_spool(filename, duration=2.5):
    num_samples = int(duration * SAMPLE_RATE)
    samples = []
    
    for i in range(num_samples):
        t = i / duration # 0 to 1
        # Turbo frequency sweeps from 950 Hz up to 3400 Hz
        freq = 950.0 + 2450.0 * (t ** 1.8)
        
        # Spool whistle + airflow rush
        phase = 2.0 * math.pi * freq * (i / SAMPLE_RATE)
        whistle = 0.35 * math.sin(phase)
        airflow = 0.20 * (random.random() * 2.0 - 1.0) * math.sin(phase * 0.5)
        
        # Volume envelope (swell in middle, taper at end)
        env = math.sin(t * math.pi)
        samples.append((whistle + airflow) * env * 0.55)
        
    return write_wav(filename, samples, channels=1)

def generate_air_purge(filename, duration=1.2):
    num_samples = int(duration * SAMPLE_RATE)
    samples = []
    
    for i in range(num_samples):
        t = i / SAMPLE_RATE
        # Exponential decaying hiss with 1400 Hz resonance
        decay = math.exp(-3.5 * t)
        hiss = (random.random() * 2.0 - 1.0) * decay
        chuff = 0.4 * math.sin(2.0 * math.pi * 140.0 * t) * math.exp(-12.0 * t)
        samples.append((hiss * 0.75 + chuff) * 0.85)
        
    return write_wav(filename, samples, channels=1)

def generate_air_horn(filename, duration=2.2):
    num_samples = int(duration * SAMPLE_RATE)
    samples = []
    
    # Authentic Indian 3-Tone Brass Trumpet Chord (F#4: 370 Hz, A#4: 466 Hz, C#5: 554 Hz)
    f1, f2, f3 = 370.0, 466.0, 554.0
    
    for i in range(num_samples):
        t = i / SAMPLE_RATE
        
        # Smooth attack/decay envelope
        env = 1.0
        if t < 0.08:
            env = t / 0.08
        elif t > duration - 0.3:
            env = (duration - t) / 0.3
            
        t1 = math.sin(2.0 * math.pi * f1 * t)
        t2 = 0.85 * math.sin(2.0 * math.pi * f2 * t + 0.2)
        t3 = 0.75 * math.sin(2.0 * math.pi * f3 * t + 0.5)
        
        chord = (t1 + t2 + t3) / 2.6
        # Stereo left/right panning
        samples.append(chord * env * 0.80) # Left
        samples.append(chord * env * 0.80) # Right
        
    return write_wav(filename, samples, channels=2)

def generate_retarder(filename, duration=3.0):
    num_samples = int(duration * SAMPLE_RATE)
    samples = []
    
    for i in range(num_samples):
        t = i / SAMPLE_RATE
        # Hydrodynamic stator fluid rush (450 Hz + 900 Hz whine)
        whine = 0.40 * math.sin(2.0 * math.pi * 480.0 * t) + 0.25 * math.sin(2.0 * math.pi * 960.0 * t)
        fluid = 0.25 * (random.random() * 2.0 - 1.0)
        
        fade = 1.0
        if i < 2000: fade = i / 2000.0
        elif i > num_samples - 2000: fade = (num_samples - i) / 2000.0
        
        samples.append((whine + fluid) * fade * 0.55)
        
    return write_wav(filename, samples, channels=1)

def generate_tire_road(filename, duration=4.0):
    num_samples = int(duration * SAMPLE_RATE)
    samples = []
    
    for i in range(num_samples):
        t = i / SAMPLE_RATE
        # Low frequency rumble (45 Hz) + white noise road hiss (filtered)
        rumble = 0.45 * math.sin(2.0 * math.pi * 45.0 * t)
        tread = 0.25 * math.sin(2.0 * math.pi * 180.0 * t)
        hiss = 0.15 * (random.random() * 2.0 - 1.0)
        
        fade = 1.0
        if i < 2000: fade = i / 2000.0
        elif i > num_samples - 2000: fade = (num_samples - i) / 2000.0
        
        s = (rumble + tread + hiss) * fade * 0.60
        samples.append(s) # Left
        samples.append(s) # Right
        
    return write_wav(filename, samples, channels=2)

def generate_relay_click(filename, duration=0.1):
    num_samples = int(duration * SAMPLE_RATE)
    samples = []
    for i in range(num_samples):
        t = i / SAMPLE_RATE
        click = math.sin(2.0 * math.pi * 2400.0 * t) * math.exp(-60.0 * t)
        samples.append(click * 0.70)
    return write_wav(filename, samples, channels=1)

def generate_gear_clunk(filename, duration=0.35):
    num_samples = int(duration * SAMPLE_RATE)
    samples = []
    for i in range(num_samples):
        t = i / SAMPLE_RATE
        clunk = (math.sin(2.0 * math.pi * 120.0 * t) + 0.5 * math.sin(2.0 * math.pi * 320.0 * t)) * math.exp(-18.0 * t)
        samples.append(clunk * 0.80)
    return write_wav(filename, samples, channels=1)

def generate_door_actuate(filename, duration=1.8):
    num_samples = int(duration * SAMPLE_RATE)
    samples = []
    for i in range(num_samples):
        t = i / SAMPLE_RATE
        air_stroke = (random.random() * 2.0 - 1.0) * math.exp(-2.0 * t) * 0.35
        seal_click = math.sin(2.0 * math.pi * 800.0 * (t - 1.6)) * math.exp(-25.0 * abs(t - 1.6)) if t > 1.5 else 0.0
        samples.append((air_stroke + seal_click) * 0.75)
    return write_wav(filename, samples, channels=1)

def generate_compressor(filename, duration=3.0):
    num_samples = int(duration * SAMPLE_RATE)
    samples = []
    for i in range(num_samples):
        t = i / SAMPLE_RATE
        piston = 0.5 * math.sin(2.0 * math.pi * 28.0 * t) ** 2 + 0.3 * math.sin(2.0 * math.pi * 56.0 * t)
        fade = 1.0
        if i < 2000: fade = i / 2000.0
        elif i > num_samples - 2000: fade = (num_samples - i) / 2000.0
        samples.append(piston * fade * 0.50)
    return write_wav(filename, samples, channels=1)

def build_all_audio():
    print("==================================================")
    print("  BUSSIGO V2 — GENERATING 44.1kHz WAV AUDIO SAMPLES")
    print("==================================================")
    
    # 1. Engine Layers (Base HZ = RPM / 60)
    generate_engine_layer("Engine_Diesel_Idle_Loop.wav", duration=4.0, base_hz=650.0/60.0) # ~10.8 Hz -> 32.5 Hz firing
    generate_engine_layer("Engine_Diesel_LowRPM_Loop.wav", duration=4.0, base_hz=1100.0/60.0) # ~18.3 Hz -> 55 Hz firing
    generate_engine_layer("Engine_Diesel_MidRPM_Loop.wav", duration=4.0, base_hz=1600.0/60.0) # ~26.6 Hz -> 80 Hz firing
    generate_engine_layer("Engine_Diesel_HighRPM_Loop.wav", duration=4.0, base_hz=2200.0/60.0) # ~36.6 Hz -> 110 Hz firing
    
    # 2. Powertrain, Turbo, Transmission
    generate_turbo_spool("Turbo_Spool_Whistle.wav", duration=2.5)
    generate_gear_clunk("Transmission_GearShift_Clunk.wav", duration=0.35)
    generate_retarder("Retarder_Hydrodynamic_Loop.wav", duration=3.0)
    
    # 3. Pneumatics & Brakes
    generate_air_purge("AirBrake_Release_Purge.wav", duration=1.2)
    generate_compressor("Compressor_Recharge_Loop.wav", duration=3.0)
    
    # 4. Actuators, Horn, Tyres
    generate_door_actuate("Door_Pneumatic_Actuate.wav", duration=1.8)
    generate_relay_click("Indicator_Relay_Click.wav", duration=0.1)
    generate_air_horn("AirHorn_Musical_Chord.wav", duration=2.2)
    generate_tire_road("Tire_Asphalt_Rolling_Loop.wav", duration=4.0)
    
    print("\nALL 11 ACOUSTIC AUDIO ASSETS GENERATED SUCCESSFULLY!")

if __name__ == "__main__":
    build_all_audio()
