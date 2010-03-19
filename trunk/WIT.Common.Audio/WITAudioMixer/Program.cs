﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NAudio.Wave;
using WIT.Common.Helpers.Text;

namespace WITAudioMixer
{
    class Program
    {
        static void Main(string[] args)
        {
            ConsoleArgumentsParser parameters = new ConsoleArgumentsParser(args);

            string audioFile1 = String.Empty,
                audioFile2 = String.Empty,
                offset = String.Empty,
                initialSilenceMs = "0",
                outputFile = String.Empty;

            if (parameters["audioFile1"] != null)
            {
                audioFile1 = parameters["audioFile1"];
            }
            if (parameters["audioFile2"] != null)
            {
                audioFile2 = parameters["audioFile2"];
            }
            if (parameters["offset"] != null)
            {
                offset = parameters["offset"];
            }
            if (parameters["initialSilenceMs"] != null)
            {
                initialSilenceMs = parameters["initialSilenceMs"];
            }
            if (parameters["outputFile"] != null)
            {
                outputFile = parameters["outputFile"];
            }

            Program.mergeAudios(audioFile1, audioFile2, long.Parse(offset), long.Parse(initialSilenceMs), outputFile);

        }

        private static void mergeAudios(
            string audioFile1, string audioFile2, long offset, long initialSilenceMs, string outputFile)
        {
            var audio1 = new WaveFileReader(audioFile1);
            var audio2 = new WaveFileReader(audioFile2);

            // Create mixer.
            var mixer = new WaveMixerStream32();
            mixer.AutoStop = true; // Not sure if this is needed but it seemed safer to have it.

            // Offset audio 1 for mixing.
            var introSilence = TimeSpan.FromMilliseconds(initialSilenceMs);
            var audio1Offseted = new WaveOffsetStream(audio1, introSilence, TimeSpan.Zero, audio1.TotalTime);

            // Offset audio 2 for mixing.
            var audio2Offset = introSilence + TimeSpan.FromMilliseconds(offset);
            var audio2Offsetted = new WaveOffsetStream(audio2, audio2Offset, TimeSpan.Zero, audio2.TotalTime);

            // Add audios to the mixer turning them into non-padding 32bit ieee wavs; it's the only thing the mixer can handle.
            var intro32 = new WaveChannel32(audio1Offseted);
            intro32.PadWithZeroes = false;
            mixer.AddInputStream(intro32);
            var outro32 = new WaveChannel32(audio2Offsetted);
            outro32.PadWithZeroes = false;
            mixer.AddInputStream(outro32);

            var tempwav = outputFile;
            WaveFileWriter.CreateWaveFile(tempwav, new Wave32To16Stream(mixer));
        }
    }
}