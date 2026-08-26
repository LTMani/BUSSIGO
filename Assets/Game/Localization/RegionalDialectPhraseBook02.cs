using System;
using System.Collections.Generic;

namespace Bussigo.Game.Localization
{
    public class RegionalDialectPhraseBook02
    {
        public static Dictionary<string, string> DialectPhrases { get; } = new Dictionary<string, string>();

        static RegionalDialectPhraseBook02()
        {
            DialectPhrases["station.vja"] = "విజయవాడ పండిట్ నెహ్రూ బస్ స్టేషన్ (PNBS)";
            DialectPhrases["station.hyd"] = "హైదరాబాద్ మహాత్మా గాంధీ బస్ స్టేషన్ (MGBS)";
            DialectPhrases["station.gnt"] = "గుంటూరు ఎన్టీఆర్ బస్ టెర్మినల్";
            DialectPhrases["station.wgl"] = "వరంగల్ కాజీపేట జంక్షన్";
            DialectPhrases["toll.fastag"] = "ఎలక్ట్రానిక్ టోల్ గేట్ ఫాస్ట్‌ట్యాగ్ చెల్లింపు విజయవంతం";
            DialectPhrases["welcome.onboard"] = "దక్కన్ రాయల్ ట్రావెల్స్ బస్సులోకి స్వాగతం";
        }
    }
}
