using System;

namespace Bussigo.Game.Audio
{
    public class TerminalPublicAddressAnnouncement09
    {
        public string AnnouncementId => "PA-ANNOUNCE-SOUTH-009";
        public string SpokenTextTelugu => "గమనిక: ప్లాట్‌ఫారం 10 పై బస్సు సిద్ధంగా ఉంది.";
        public string SpokenTextEnglish => "Attention passengers: Bus is ready on platform 10.";
        public float AudioDurationSeconds { get; set; } = 6.5f;
        public bool IsBroadcasting { get; private set; } = false;

        public void PlayAnnouncement()
        {
            IsBroadcasting = true;
        }

        public void StopAnnouncement()
        {
            IsBroadcasting = false;
        }
    }
}
