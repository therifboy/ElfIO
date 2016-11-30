using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ElfIO
{
    public class Elf
    {
        ElfHDR header;
        List<ElfProgram> programs;
        List<ElfSection> sections;

        public Elf(ElfHDR elfhdr, ElfProgram[] programs, ElfSection[] sections)
        {
            // Initialize elf header
            this.header = elfhdr;

            // Initialize program headers
            this.programs = new List<ElfProgram>(programs);

            // Initialize section headers
            this.sections = new List<ElfSection>(sections);
        }

        public ElfHDR Header
        {
            get
            {
                return header;
            }
            set
            {
                header = value;
            }
        }

        public List<ElfProgram> Programs
        {
            get
            {
                return programs;
            }
            set
            {
                programs = value;
            }
        }

        public List<ElfSection> Sections
        {
            get
            {
                return sections;
            }
            set
            {
                sections = value;
            }
        }

        #region DEBUG
        public void DumpString()
        {
            ElfHDR elfhdr = header;
            ElfPHDR elfphdr;
            ElfSHDR elfshdr;

            // Initialze output file
            System.IO.BinaryWriter writer = new System.IO.BinaryWriter(System.IO.File.Create("dump.txt"));

            // Write Elf
            writer.Write("****ELF HEADER****\n");
            writer.Write(String.Format("ID: {0}\nType: {1}\nMachine: {2}\nVersion: {3}\nEntryPoint: 0x{4:X8}\nPHOffset: 0x{5:X8}\nSHOffset: 0x{6:X8}\nFlags: 0x{7:X4}\nHeaderSize: 0x{8:X2}\nPHSize: 0x{9:X2}\nPHCount: 0x{10:X2}\nSHSize: 0x{11:X2}\nSHCount: 0x{12:X2}\nStringTable: 0x{13:X2}\n\n",
                             BitConverter.ToString(elfhdr.Identification),
                             elfhdr.Type.ToString(),
                             elfhdr.MachineType.ToString(),
                             elfhdr.Version.ToString(),
                             elfhdr.EntryPoint,
                             elfhdr.ProgramHeaderOffset,
                             elfhdr.SectionHeaderOffset,
                             elfhdr.Flags,
                             elfhdr.HeaderSize,
                             elfhdr.ProgramHeaderSize,
                             elfhdr.ProgramHeaderCount,
                             elfhdr.SectionHeaderSize,
                             elfhdr.SectionHeaderCount,
                             elfhdr.StringTable));

            // Write PHDR
            writer.Write("****PROGRAM HEADERS****\n");
            for (int i = 0; i < elfhdr.ProgramHeaderCount; i++)
            {
                elfphdr = programs[i].Header;
                writer.Write(String.Format("Program[{10}]\nType = {0}\nFlags = {1}\nOffset = 0x{2:X8} => 0x{8:X8} => 0x{9:X8}\nVaddr = 0x{3:X8}\nPaddr = 0x{4:X8}\nFilesz = 0x{5:X8}\nMemsz = 0x{6:X8}\nAlign = 0x{7:X8}\n\n",
                            elfphdr.Type.ToString(),
                            elfphdr.Flags.ToString(),
                            elfphdr.FileOffset,
                            elfphdr.VirtualAddress,
                            elfphdr.PhysicalAddress,
                            elfphdr.FileSize,
                            elfphdr.MemorySize,
                            elfphdr.Align,
                            elfphdr.FileOffset + elfphdr.FileSize,
                            (elfphdr.FileOffset + elfphdr.FileSize + elfphdr.Align - 1) & ~(elfphdr.Align - 1),
                            i));
            }

            // WRITE SHDR
            writer.Write("****SECTION HEADERS****\n");
            for (int i = 0; i < elfhdr.SectionHeaderCount; i++)
            {
                elfshdr = sections[i].Header;
                writer.Write(String.Format("Section[{12}]\nName: 0x{0:X4}\nType: {1}\nFlags: {2}\nAddress: 0x{3:X8}\nOffset: 0x{4:X8} => 0x{10:X8} => 0x{11:X8}\nSize: 0x{5:X8}\nLink: 0x{6:X4}\nInfo: 0x{7:X4}\nAlign: 0x{8:X8}\nEntries: 0x{9:X8}\n\n",
                            elfshdr.Name,
                            elfshdr.Type.ToString(),
                            elfshdr.Flags.ToString(),
                            elfshdr.Address,
                            elfshdr.FileOffset,
                            elfshdr.Size,
                            elfshdr.Link,
                            elfshdr.Info,
                            elfshdr.Align,
                            elfshdr.EntrySize,
                            elfshdr.FileOffset + elfshdr.Size,
                            (elfshdr.FileOffset + elfshdr.Size + elfshdr.Align - 1) & ~(elfshdr.Align - 1),
                            i));
            }

            // Close writer
            writer.Close();
            writer.Dispose();
        }
        #endregion
    }
}