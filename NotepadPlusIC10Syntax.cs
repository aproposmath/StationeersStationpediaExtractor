using System;
using System.Linq;

using Assets.Scripts;
using Assets.Scripts.Objects.Electrical;
using Assets.Scripts.Objects.Motherboards;

namespace DataExtractor
{
    static class NotepadPlusLanguageExporter
    {
        public static void Export()
        {
            string instructions = string.Join(" ", EnumCollections.ScriptCommands.Values.Select(cmd => Enum.GetName(typeof(ScriptCommand), cmd)));
            string logicTypes = string.Join(" ", EnumCollections.LogicTypes.Values.Select(lt => Enum.GetName(typeof(LogicType), lt)));

            string registers = string.Join(" ", Enumerable.Range(0, 18).Select(i => "r" + i)) + " " +
                               string.Join(" ", Enumerable.Range(0, 18).Select(i => "rr" + i)) + " sp ra";

            string devices = string.Join(" ", Enumerable.Range(0, 6).Select(i => "d" + i)) + " " +
                             string.Join(" ", Enumerable.Range(0, 18).Select(i => "dr" + i));

            string output = Template
                  .Replace("{{INSTRUCTIONS}}", instructions)
                  .Replace("{{LOGIC_TYPES}}", logicTypes)
                  .Replace("{{REGISTERS}}", registers)
                  .Replace("{{DEVICES}}", devices);

            System.IO.File.WriteAllText("data/notepadplus_ic10.xml", output);
        }

        // Template provided by SemlerPDX
        // https://discord.com/channels/276525882049429515/392785541411635201/1336457393520902246
        public const string Template = """
<NotepadPlus>
    <UserLang name="IC10" ext="ic10" udlVersion="2.1">
        <Settings>
            <Global caseIgnored="no" allowFoldOfComments="no" foldCompact="yes" forcePureLC="0" decimalSeparator="0" />
            <Prefix Keywords1="no" Keywords2="no" Keywords3="no" Keywords4="no" Keywords5="no" Keywords6="no" Keywords7="no" Keywords8="no" />
        </Settings>
        <KeywordLists>
            <Keywords name="Comments">00# 01 02 03 04</Keywords>
            <Keywords name="Numbers, prefix1"></Keywords>
            <Keywords name="Numbers, prefix2"></Keywords>
            <Keywords name="Numbers, extras1"></Keywords>
            <Keywords name="Numbers, extras2"></Keywords>
            <Keywords name="Numbers, suffix1"></Keywords>
            <Keywords name="Numbers, suffix2"></Keywords>
            <Keywords name="Numbers, range"></Keywords>
            <Keywords name="Operators1">&apos; &amp; ( ) * , / | + &lt; &gt;</Keywords>
            <Keywords name="Operators2"></Keywords>
            <Keywords name="Folders in code1, open">#begin</Keywords>
            <Keywords name="Folders in code1, middle"></Keywords>
            <Keywords name="Folders in code1, close">#end</Keywords>
            <Keywords name="Folders in code2, open"></Keywords>
            <Keywords name="Folders in code2, middle"></Keywords>
            <Keywords name="Folders in code2, close"></Keywords>
            <Keywords name="Folders in comment, open"></Keywords>
            <Keywords name="Folders in comment, middle"></Keywords>
            <Keywords name="Folders in comment, close"></Keywords>
            <Keywords name="Keywords1">{{INSTRUCTIONS}}</Keywords>
            <Keywords name="Keywords2">{{DEVICES}}</Keywords>
            <Keywords name="Keywords3">{{LOGIC_TYPES}}</Keywords>
            <Keywords name="Keywords4">{{REGISTERS}}</Keywords>
            <Keywords name="Keywords5">$INDEX $RANDOM $ENTRY_LO $ENTRY_LO1 $CONTEXT $PAGEMASK $WIRED $ERROR $BAD_V_ADDR $COUNT $ENTRY_HI $COMPARE $STATUS $CAUSE $EPC $PRID $CONFIG $LLADDR $WATCH_LO $WATCH_HI $ECC $CACHE_ERR $TAG_LO $TAG_HI $ERROR_EPC</Keywords>
            <Keywords name="Keywords6"></Keywords>
            <Keywords name="Keywords7"></Keywords>
            <Keywords name="Keywords8"></Keywords>
            <Keywords name="Delimiters">00HASH( 01 02) 03 04 05 06&quot; 07 08&quot; 09 10 11 12 13 14 15 16 17 18_ 19 20(( : EOL)) 21 22 23</Keywords>
        </KeywordLists>
        <Styles>
            <WordsStyle name="DEFAULT" fgColor="C0C0C0" bgColor="FFFFFF" colorStyle="1" fontStyle="0" nesting="0" />
            <WordsStyle name="COMMENTS" fgColor="00FFFF" bgColor="FFFFFF" colorStyle="1" fontStyle="0" nesting="0" />
            <WordsStyle name="LINE COMMENTS" fgColor="808080" bgColor="FFFFFF" colorStyle="1" fontStyle="0" nesting="0" />
            <WordsStyle name="NUMBERS" fgColor="00A4A4" bgColor="FFFFFF" colorStyle="1" fontStyle="0" nesting="0" />
            <WordsStyle name="KEYWORDS1" fgColor="FFFF00" bgColor="FFFFFF" colorStyle="1" fontStyle="0" nesting="0" />
            <WordsStyle name="KEYWORDS2" fgColor="00FFFF" bgColor="FFFFFF" colorStyle="1" fontStyle="0" nesting="0" />
            <WordsStyle name="KEYWORDS3" fgColor="FF8000" bgColor="FFFFFF" colorStyle="1" fontStyle="0" nesting="0" />
            <WordsStyle name="KEYWORDS4" fgColor="0080FF" bgColor="FFFFFF" colorStyle="1" fontStyle="0" nesting="0" />
            <WordsStyle name="KEYWORDS5" fgColor="FF80FF" bgColor="FFFFFF" colorStyle="1" fontStyle="0" nesting="0" />
            <WordsStyle name="KEYWORDS6" fgColor="0080FF" bgColor="FFFFFF" colorStyle="1" fontStyle="0" nesting="0" />
            <WordsStyle name="KEYWORDS7" fgColor="FF00FF" bgColor="FFFFFF" colorStyle="1" fontStyle="0" nesting="0" />
            <WordsStyle name="KEYWORDS8" fgColor="000000" bgColor="FFFFFF" colorStyle="1" fontStyle="0" nesting="0" />
            <WordsStyle name="OPERATORS" fgColor="00FFFF" bgColor="FFFFFF" colorStyle="1" fontStyle="0" nesting="0" />
            <WordsStyle name="FOLDER IN CODE1" fgColor="0080FF" bgColor="FFFFFF" colorStyle="1" fontStyle="0" nesting="0" />
            <WordsStyle name="FOLDER IN CODE2" fgColor="000000" bgColor="FFFFFF" colorStyle="1" fontStyle="0" nesting="0" />
            <WordsStyle name="FOLDER IN COMMENT" fgColor="000000" bgColor="FFFFFF" colorStyle="1" fontStyle="0" nesting="0" />
            <WordsStyle name="DELIMITERS1" fgColor="B7B700" bgColor="FFFFFF" colorStyle="1" fontStyle="0" nesting="4" />
            <WordsStyle name="DELIMITERS2" fgColor="FF0000" bgColor="FFFFFF" colorStyle="1" fontStyle="0" nesting="0" />
            <WordsStyle name="DELIMITERS3" fgColor="FF8040" bgColor="FFFFFF" colorStyle="1" fontStyle="0" nesting="1" />
            <WordsStyle name="DELIMITERS4" fgColor="FF0080" bgColor="FFFFFF" colorStyle="1" fontStyle="0" nesting="0" />
            <WordsStyle name="DELIMITERS5" fgColor="000000" bgColor="FFFFFF" colorStyle="1" fontStyle="0" nesting="0" />
            <WordsStyle name="DELIMITERS6" fgColor="000000" bgColor="FFFFFF" colorStyle="1" fontStyle="0" nesting="0" />
            <WordsStyle name="DELIMITERS7" fgColor="FF00FF" bgColor="FFFFFF" colorStyle="1" fontStyle="0" nesting="0" />
            <WordsStyle name="DELIMITERS8" fgColor="000000" bgColor="FFFFFF" colorStyle="1" fontStyle="0" nesting="0" />
        </Styles>
    </UserLang>
</NotepadPlus>
""";
    }

}
