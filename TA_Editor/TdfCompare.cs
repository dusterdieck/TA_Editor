namespace TA_Editor
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;

    using TAUtil.Tdf;


    public static class TdfCompare
    {
        public interface IInstruction
        {
            int StartIndex { get; }
        }

        public class Replace : IInstruction
        {
            public int StartIndex { get; set; }

            public int EndIndex { get; set; }

            public string NewValue { get; set; }
        }

        public class Insert : IInstruction
        {
            public int StartIndex { get; set; }

            public string Value { get; set; }
        }
        public class InstructionComparer : IComparer<IInstruction>
        {
            public int Compare(IInstruction x, IInstruction y)
            {
                return x.StartIndex - y.StartIndex;
            }
        }

        private static bool isEmptyOrDefault(string s)
        {
            if (string.IsNullOrWhiteSpace(s))
            {
                return true;
            }

            if (TdfConvert.TryToDouble(s, out var v))
            {
                if (v == 0.0)
                {
                    return true;
                }
            }

            return false;
        }

        public static List<IInstruction> ComputePropertyMapping(TdfNode from, TdfNode to, int indentLevel)
        {
            var lastPropertyEnd = from.Entries.Values.Max(x => x.EndIndex);
            var instructions = new List<IInstruction>();

            foreach (var entry in to.Entries)
            {
                if (from.Entries.TryGetValue(entry.Key, out var otherValue))
                {
                    if (isEmptyOrDefault(entry.Value.Value) && isEmptyOrDefault(otherValue.Value))
                    {
                        continue;
                    }

                    if (entry.Value.Value == otherValue.Value)
                    {
                        continue;
                    }
                    instructions.Add(new Replace
                        {
                            StartIndex = otherValue.ValueStartIndex,
                            EndIndex = otherValue.ValueEndIndex,
                            NewValue = entry.Value.Value
                        });
                }
                else if (!isEmptyOrDefault(entry.Value.Value))
                {
                    instructions.Add(new Insert
                        {
                            StartIndex = lastPropertyEnd,
                            Value = $"{entry.Key}={entry.Value.Value};\r\n" + new string('\t', indentLevel)
                        });
                }
            }

            return instructions;
        }

        public static void PerformInstructions(string file, IEnumerable<IInstruction> instructions)
        {
            var tempFile = $"{file}.__TA_Editor_tmp";
            using (var input = new StreamReader(file, Encoding.GetEncoding(1252)))
            {
                using (var output = new StreamWriter(File.OpenWrite(tempFile), Encoding.GetEncoding(1252)))
                {
                    PerformInstructions(input, output, instructions);
                }
            }
            File.Replace(tempFile, file, null);
        }

        public static void PerformInstructions(
            TextReader input,
            TextWriter output,
            IEnumerable<IInstruction> instructions)
        {
            var sortedInstructions = instructions.ToList();
            sortedInstructions.Sort(new InstructionComparer());

            int index = 0;
            foreach (var instruction in sortedInstructions)
            {
                while (index < instruction.StartIndex)
                {
                    var c = input.Read();
                    if (c == -1)
                    {
                        throw new Exception("File ended before instructions");
                    }

                    output.Write(char.ConvertFromUtf32(c));
                    ++index;
                }

                switch (instruction)
                {
                    case Replace replace:
                        {
                            output.Write(replace.NewValue);
                            while (index < replace.EndIndex)
                            {
                                input.Read();
                                ++index;
                            }

                            break;
                        }

                    case Insert insert:
                        {
                            output.Write(insert.Value);
                            break;
                        }
                }
            }

            output.Write(input.ReadToEnd());
        }
    }
}