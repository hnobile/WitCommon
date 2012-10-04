using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NAudio.Wave;
using WIT.Common.Helpers.Text;
using System.IO;

namespace WITAudioMixer
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                ConsoleArgumentsParser parameters = new ConsoleArgumentsParser(args);

                string action = String.Empty,
                    audioFile1 = String.Empty,
                    audioFile2 = String.Empty,
                    offset = String.Empty,
                    initialSilenceMs = "0",
                    outputFile = String.Empty;

                if (parameters["action"] != null)
                {
                    action = parameters["action"];
                }
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

                switch (action)
                {
                    case "merge":
                        Program.mergeAudios(audioFile1, audioFile2, long.Parse(offset), long.Parse(initialSilenceMs), outputFile);
                        break;
                    case "addSilence":
                        Program.generateAudioWithSilenceFirst(audioFile1, long.Parse(initialSilenceMs), outputFile);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex) {
                Console.WriteLine("WITAaudio Mixer Error: " + ex.Message);
            }

        }

        private static void generateAudioWithSilenceFirst(string audioFile, long initialSilenceMs, string outputFile)
        {
            var audio = new WaveFileReader(audioFile);

            // Create mixer.
            var mixer = new WaveMixerStream32();
            mixer.AutoStop = true; // Not sure if this is needed but it seemed safer to have it.

            var introSilence = TimeSpan.FromMilliseconds(initialSilenceMs);

            // Offset audios for mixing.
            var audioOffsetted = new WaveOffsetStream(audio, introSilence, TimeSpan.Zero, audio.TotalTime);

            // Add audios to the mixer turning them into non-padding 32bit ieee wavs; it's the only thing the mixer can handle.
            var audio32 = new WaveChannel32(audioOffsetted);
            audio32.PadWithZeroes = false;
            mixer.AddInputStream(audio32);

            WaveFileWriter.CreateWaveFile(outputFile, new Wave32To16Stream(mixer));
        }

        private static void mergeAudios(
            string audioFile1, string audioFile2, long offset, long initialSilenceMs, string outputFile)
        {
            WaveFileReader audio1 = null;
            WaveFileReader audio2 = null;

            try
            {
                audio1 = new WaveFileReader(audioFile1);
            }
            catch (Exception ex) { 
                //If this file cannot be read, gracefuly ignore the error because we are going to ignore the file anyway
                //Console.WriteLine("EXCEPTION GETTING AUDIO 1" + ex.Message);
            }
            try
            {
                audio2 = new WaveFileReader(audioFile2);
            }
            catch (Exception ex) {
                //If this file cannot be read, gracefuly ignore the error because we are going to ignore the file anyway
                //Console.WriteLine("EXCEPTION GETTING AUDIO 2" + ex.Message);
            }
            // Create mixer.
            var mixer = new WaveMixerStream32();
            mixer.AutoStop = true; // Not sure if this is needed but it seemed safer to have it.

            //If we have the 2 files, merge them
            if (audio1 != null && audio2 != null)
            {
                // Offset audio 1 for mixing.
                var introSilence = TimeSpan.FromMilliseconds(initialSilenceMs);
                var audio1Offseted = new WaveOffsetStream(audio1, introSilence, TimeSpan.Zero, audio1.TotalTime);

                // Offset audio 2 for mixing.
                var audio2Offset = TimeSpan.FromMilliseconds(offset);
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
            else if (audio1 == null && audio2 != null) {
                //If we only have audio2, use it as the merged result
                var outro32 = new WaveChannel32(audio2);
                outro32.PadWithZeroes = false;
                mixer.AddInputStream(outro32);
                var tempwav = outputFile;
                WaveFileWriter.CreateWaveFile(tempwav, new Wave32To16Stream(mixer));
            }
            else if (audio1 != null && audio2 == null)
            {
                //If we only have audio1, use it as the merged result
                var outro32 = new WaveChannel32(audio1);
                outro32.PadWithZeroes = false;
                mixer.AddInputStream(outro32);
                var tempwav = outputFile;
                WaveFileWriter.CreateWaveFile(tempwav, new Wave32To16Stream(mixer));
            }

            //Console.WriteLine("AUDIO SAVED");
        }
    }
}
