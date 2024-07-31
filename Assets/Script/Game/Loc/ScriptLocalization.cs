public static class ScriptLocalization
{
    public static string Get(string Term)
    {
        return LocalizationManager.GetTermTranslation(Term, LocalizationManager.IsRight2Left, 0, false);
    }

    public static string Get(string Term, bool FixForRTL)
    {
        return LocalizationManager.GetTermTranslation(Term, FixForRTL, 0, false);
    }

    public static string Get(string Term, bool FixForRTL, int maxLineLengthForRTL)
    {
        return LocalizationManager.GetTermTranslation(Term, FixForRTL, maxLineLengthForRTL, false);
    }

    public static string Get(string Term, bool FixForRTL, int maxLineLengthForRTL, bool ignoreNumbers)
    {
        return LocalizationManager.GetTermTranslation(Term, FixForRTL, maxLineLengthForRTL, ignoreNumbers);
    }

    public static class mobile
    {
        public static string e16s10 => ScriptLocalization.Get("mobile/e16s10");

        public static string e16s3 => ScriptLocalization.Get("mobile/e16s3");

        public static string e16s4 => ScriptLocalization.Get("mobile/e16s4");

        public static string e16s5 => ScriptLocalization.Get("mobile/e16s5");

        public static string e16s6 => ScriptLocalization.Get("mobile/e16s6");

        public static string e16s7 => ScriptLocalization.Get("mobile/e16s7");

        public static string e16s8 => ScriptLocalization.Get("mobile/e16s8");

        public static string e16s9 => ScriptLocalization.Get("mobile/e16s9");

        public static string e19s2 => ScriptLocalization.Get("mobile/e19s2");

        public static string e19s5 => ScriptLocalization.Get("mobile/e19s5");

        public static string e22s2 => ScriptLocalization.Get("mobile/e22s2");

        public static string e22s5 => ScriptLocalization.Get("mobile/e22s5");
    }

    public static class mobile_detailInfo
    {
        public static string _0 => ScriptLocalization.Get("mobile/detailInfo/0");

        public static string _1 => ScriptLocalization.Get("mobile/detailInfo/1");

        public static string _2 => ScriptLocalization.Get("mobile/detailInfo/2");

        public static string _3 => ScriptLocalization.Get("mobile/detailInfo/3");

        public static string _4 => ScriptLocalization.Get("mobile/detailInfo/4");

        public static string _5 => ScriptLocalization.Get("mobile/detailInfo/5");

        public static string _6 => ScriptLocalization.Get("mobile/detailInfo/6");

        public static string _7 => ScriptLocalization.Get("mobile/detailInfo/7");
    }

    public static class mobile_enhancement
    {
        public static string airAttackLv0ControlKeys => ScriptLocalization.Get("mobile/enhancement/airAttackLv0ControlKeys");

        public static string airCombo1Lv0ControlKeys => ScriptLocalization.Get("mobile/enhancement/airCombo1Lv0ControlKeys");

        public static string airCombo2Lv0ControlKeys => ScriptLocalization.Get("mobile/enhancement/airCombo2Lv0ControlKeys");

        public static string attackLv0ControlKeys => ScriptLocalization.Get("mobile/enhancement/attackLv0ControlKeys");

        public static string attackLv3ControlKeys => ScriptLocalization.Get("mobile/enhancement/attackLv3ControlKeys");

        public static string avatarAttackLv0ControlKeys => ScriptLocalization.Get("mobile/enhancement/avatarAttackLv0ControlKeys");

        public static string bladeStormLv0ControlKeys => ScriptLocalization.Get("mobile/enhancement/bladeStormLv0ControlKeys");

        public static string chargingLv0ControlKeys => ScriptLocalization.Get("mobile/enhancement/chargingLv0ControlKeys");

        public static string combo1Lv0ControlKeys => ScriptLocalization.Get("mobile/enhancement/combo1Lv0ControlKeys");

        public static string combo2Lv0ControlKeys => ScriptLocalization.Get("mobile/enhancement/combo2Lv0ControlKeys");

        public static string hitGroundLv0ControlKeys => ScriptLocalization.Get("mobile/enhancement/hitGroundLv0ControlKeys");

        public static string tripleAttackLv0ControlKeys => ScriptLocalization.Get("mobile/enhancement/tripleAttackLv0ControlKeys");
    }

    public static class mobile_story
    {
        public static string e1s2 => ScriptLocalization.Get("mobile/story/e1s2");

        public static string e1s3 => ScriptLocalization.Get("mobile/story/e1s3");
    }

    public static class mobile_tutorial
    {
        public static string back2I => ScriptLocalization.Get("mobile/tutorial/back2I");

        public static string charging => ScriptLocalization.Get("mobile/tutorial/charging");

        public static string excute => ScriptLocalization.Get("mobile/tutorial/excute");

        public static string flashAttack => ScriptLocalization.Get("mobile/tutorial/flashAttack");

        public static string jumpAcross => ScriptLocalization.Get("mobile/tutorial/jumpAcross");

        public static string ukemi => ScriptLocalization.Get("mobile/tutorial/ukemi");
    }

    public static class mobile_ui
    {
        public static string trophy => ScriptLocalization.Get("mobile/ui/trophy");
    }

    public static class mobile_ui_quality_level
    {
        public static string high => ScriptLocalization.Get("mobile/ui/quality_level/high");

        public static string low => ScriptLocalization.Get("mobile/ui/quality_level/low");

        public static string normal => ScriptLocalization.Get("mobile/ui/quality_level/normal");
    }

    public static class mobile_ui_start
    {
        public static string back => ScriptLocalization.Get("mobile/ui/start/back");

        public static string fps => ScriptLocalization.Get("mobile/ui/start/fps");

        public static string quality_level => ScriptLocalization.Get("mobile/ui/start/quality_level");

        public static string who_are_you => ScriptLocalization.Get("mobile/ui/start/who_are_you");
    }

    public static class Sprite_blood_palace
    {
        public static string PerfectWord => ScriptLocalization.Get("Sprite/blood_palace/PerfectWord");

        public static string pword => ScriptLocalization.Get("Sprite/blood_palace/pword");

        public static string PWordFlash1 => ScriptLocalization.Get("Sprite/blood_palace/PWordFlash1");

        public static string PWordFlash2 => ScriptLocalization.Get("Sprite/blood_palace/PWordFlash2");

        public static string PWordFlash3 => ScriptLocalization.Get("Sprite/blood_palace/PWordFlash3");

        public static string PWordFlashBG1 => ScriptLocalization.Get("Sprite/blood_palace/PWordFlashBG1");

        public static string PWordFlashBG2 => ScriptLocalization.Get("Sprite/blood_palace/PWordFlashBG2");

        public static string PWordFlashBG3 => ScriptLocalization.Get("Sprite/blood_palace/PWordFlashBG3");
    }

    public static class Sprite_C1L1S4
    {
        public static string forest_0000_middle_sign_1 => ScriptLocalization.Get("Sprite/C1L1S4/forest_0000_middle_sign 1");
    }

    public static class Sprite_C2L1S3
    {
        public static string C2L1_s3_china_1 => ScriptLocalization.Get("Sprite/C2L1S3/C2L1_s3_china_1");

        public static string C2L1_s3_china_2 => ScriptLocalization.Get("Sprite/C2L1S3/C2L1_s3_china_2");
    }

    public static class Sprite_C2L2S10
    {
        public static string C2L2_s10_china_2 => ScriptLocalization.Get("Sprite/C2L2S10/C2L2_s10_china_2");
    }

    public static class Sprite_C2L2S11
    {
        public static string C2L2_s11_china => ScriptLocalization.Get("Sprite/C2L2S11/C2L2_s11_china");
    }

    public static class Sprite_C4L2S8
    {
        public static string C2L1_s3_china_1 => ScriptLocalization.Get("Sprite/C4L2S8/C2L1_s3_china_1");

        public static string C2L1_s3_china_2
        {
            get { return ScriptLocalization.Get("Sprite/C4L2S8/C2L1_s3_china_2"); }
        }
    }

    public static class Sprite_start
    {
        public static string ICEY
        {
            get { return ScriptLocalization.Get("Sprite/start/ICEY"); }
        }
    }

    public static class Story
    {
        public static string e10s1
        {
            get { return ScriptLocalization.Get("Story/e10s1"); }
        }

        public static string e10s10
        {
            get { return ScriptLocalization.Get("Story/e10s10"); }
        }

        public static string e10s11
        {
            get { return ScriptLocalization.Get("Story/e10s11"); }
        }

        public static string e10s12
        {
            get { return ScriptLocalization.Get("Story/e10s12"); }
        }

        public static string e10s13
        {
            get { return ScriptLocalization.Get("Story/e10s13"); }
        }

        public static string e10s14
        {
            get { return ScriptLocalization.Get("Story/e10s14"); }
        }

        public static string e10s15
        {
            get { return ScriptLocalization.Get("Story/e10s15"); }
        }

        public static string e10s2
        {
            get { return ScriptLocalization.Get("Story/e10s2"); }
        }

        public static string e10s3
        {
            get { return ScriptLocalization.Get("Story/e10s3"); }
        }

        public static string e10s4
        {
            get { return ScriptLocalization.Get("Story/e10s4"); }
        }

        public static string e10s5
        {
            get { return ScriptLocalization.Get("Story/e10s5"); }
        }

        public static string e10s6
        {
            get { return ScriptLocalization.Get("Story/e10s6"); }
        }

        public static string e10s7
        {
            get { return ScriptLocalization.Get("Story/e10s7"); }
        }

        public static string e10s9
        {
            get { return ScriptLocalization.Get("Story/e10s9"); }
        }

        public static string e11s1
        {
            get { return ScriptLocalization.Get("Story/e11s1"); }
        }

        public static string e11s2
        {
            get { return ScriptLocalization.Get("Story/e11s2"); }
        }

        public static string e11s3
        {
            get { return ScriptLocalization.Get("Story/e11s3"); }
        }

        public static string e11s4
        {
            get { return ScriptLocalization.Get("Story/e11s4"); }
        }

        public static string e11s5
        {
            get { return ScriptLocalization.Get("Story/e11s5"); }
        }

        public static string e12s1
        {
            get { return ScriptLocalization.Get("Story/e12s1"); }
        }

        public static string e12s10
        {
            get { return ScriptLocalization.Get("Story/e12s10"); }
        }

        public static string e12s11
        {
            get { return ScriptLocalization.Get("Story/e12s11"); }
        }

        public static string e12s12
        {
            get { return ScriptLocalization.Get("Story/e12s12"); }
        }

        public static string e12s13
        {
            get { return ScriptLocalization.Get("Story/e12s13"); }
        }

        public static string e12s14
        {
            get { return ScriptLocalization.Get("Story/e12s14"); }
        }

        public static string e12s2
        {
            get { return ScriptLocalization.Get("Story/e12s2"); }
        }

        public static string e12s3
        {
            get { return ScriptLocalization.Get("Story/e12s3"); }
        }

        public static string e12s5
        {
            get { return ScriptLocalization.Get("Story/e12s5"); }
        }

        public static string e12s6
        {
            get { return ScriptLocalization.Get("Story/e12s6"); }
        }

        public static string e12s7
        {
            get { return ScriptLocalization.Get("Story/e12s7"); }
        }

        public static string e12s8
        {
            get { return ScriptLocalization.Get("Story/e12s8"); }
        }

        public static string e12s9
        {
            get { return ScriptLocalization.Get("Story/e12s9"); }
        }

        public static string e13s1
        {
            get { return ScriptLocalization.Get("Story/e13s1"); }
        }

        public static string e13s2
        {
            get { return ScriptLocalization.Get("Story/e13s2"); }
        }

        public static string e14s1
        {
            get { return ScriptLocalization.Get("Story/e14s1"); }
        }

        public static string e14s2_0
        {
            get { return ScriptLocalization.Get("Story/e14s2.0"); }
        }

        public static string e14s2_1
        {
            get { return ScriptLocalization.Get("Story/e14s2.1"); }
        }

        public static string e14s2_10
        {
            get { return ScriptLocalization.Get("Story/e14s2.10"); }
        }

        public static string e14s2_11
        {
            get { return ScriptLocalization.Get("Story/e14s2.11"); }
        }

        public static string e14s2_12
        {
            get { return ScriptLocalization.Get("Story/e14s2.12"); }
        }

        public static string e14s2_13
        {
            get { return ScriptLocalization.Get("Story/e14s2.13"); }
        }

        public static string e14s2_14
        {
            get { return ScriptLocalization.Get("Story/e14s2.14"); }
        }

        public static string e14s2_15
        {
            get { return ScriptLocalization.Get("Story/e14s2.15"); }
        }

        public static string e14s2_16
        {
            get { return ScriptLocalization.Get("Story/e14s2.16"); }
        }

        public static string e14s2_17
        {
            get { return ScriptLocalization.Get("Story/e14s2.17"); }
        }

        public static string e14s2_18
        {
            get { return ScriptLocalization.Get("Story/e14s2.18"); }
        }

        public static string e14s2_19
        {
            get { return ScriptLocalization.Get("Story/e14s2.19"); }
        }

        public static string e14s2_2
        {
            get { return ScriptLocalization.Get("Story/e14s2.2"); }
        }

        public static string e14s2_3
        {
            get { return ScriptLocalization.Get("Story/e14s2.3"); }
        }

        public static string e14s2_4
        {
            get { return ScriptLocalization.Get("Story/e14s2.4"); }
        }

        public static string e14s2_5
        {
            get { return ScriptLocalization.Get("Story/e14s2.5"); }
        }

        public static string e14s2_6
        {
            get { return ScriptLocalization.Get("Story/e14s2.6"); }
        }

        public static string e14s2_7
        {
            get { return ScriptLocalization.Get("Story/e14s2.7"); }
        }

        public static string e14s2_8
        {
            get { return ScriptLocalization.Get("Story/e14s2.8"); }
        }

        public static string e14s2_9
        {
            get { return ScriptLocalization.Get("Story/e14s2.9"); }
        }

        public static string e15s1
        {
            get { return ScriptLocalization.Get("Story/e15s1"); }
        }

        public static string e16log1
        {
            get { return ScriptLocalization.Get("Story/e16log1"); }
        }

        public static string e16log2
        {
            get { return ScriptLocalization.Get("Story/e16log2"); }
        }

        public static string e16log3
        {
            get { return ScriptLocalization.Get("Story/e16log3"); }
        }

        public static string e16log4
        {
            get { return ScriptLocalization.Get("Story/e16log4"); }
        }

        public static string e16s1
        {
            get { return ScriptLocalization.Get("Story/e16s1"); }
        }

        public static string e16s10
        {
            get { return ScriptLocalization.Get("Story/e16s10"); }
        }

        public static string e16s11
        {
            get { return ScriptLocalization.Get("Story/e16s11"); }
        }

        public static string e16s12
        {
            get { return ScriptLocalization.Get("Story/e16s12"); }
        }

        public static string e16s13
        {
            get { return ScriptLocalization.Get("Story/e16s13"); }
        }

        public static string e16s14
        {
            get { return ScriptLocalization.Get("Story/e16s14"); }
        }

        public static string e16s2
        {
            get { return ScriptLocalization.Get("Story/e16s2"); }
        }

        public static string e16s3
        {
            get { return ScriptLocalization.Get("Story/e16s3"); }
        }

        public static string e16s4
        {
            get { return ScriptLocalization.Get("Story/e16s4"); }
        }

        public static string e16s5
        {
            get { return ScriptLocalization.Get("Story/e16s5"); }
        }

        public static string e16s6
        {
            get { return ScriptLocalization.Get("Story/e16s6"); }
        }

        public static string e16s7
        {
            get { return ScriptLocalization.Get("Story/e16s7"); }
        }

        public static string e16s8
        {
            get { return ScriptLocalization.Get("Story/e16s8"); }
        }

        public static string e16s9
        {
            get { return ScriptLocalization.Get("Story/e16s9"); }
        }

        public static string e18s1
        {
            get { return ScriptLocalization.Get("Story/e18s1"); }
        }

        public static string e18s2
        {
            get { return ScriptLocalization.Get("Story/e18s2"); }
        }

        public static string e18s3
        {
            get { return ScriptLocalization.Get("Story/e18s3"); }
        }

        public static string e18s4
        {
            get { return ScriptLocalization.Get("Story/e18s4"); }
        }

        public static string e18s5
        {
            get { return ScriptLocalization.Get("Story/e18s5"); }
        }

        public static string e19s1
        {
            get { return ScriptLocalization.Get("Story/e19s1"); }
        }

        public static string e19s2
        {
            get { return ScriptLocalization.Get("Story/e19s2"); }
        }

        public static string e19s3
        {
            get { return ScriptLocalization.Get("Story/e19s3"); }
        }

        public static string e19s4
        {
            get { return ScriptLocalization.Get("Story/e19s4"); }
        }

        public static string e19s5
        {
            get { return ScriptLocalization.Get("Story/e19s5"); }
        }

        public static string e19s6
        {
            get { return ScriptLocalization.Get("Story/e19s6"); }
        }

        public static string e1s1
        {
            get { return ScriptLocalization.Get("Story/e1s1"); }
        }

        public static string e1s2
        {
            get { return ScriptLocalization.Get("Story/e1s2"); }
        }

        public static string e1s3
        {
            get { return ScriptLocalization.Get("Story/e1s3"); }
        }

        public static string e21s1
        {
            get { return ScriptLocalization.Get("Story/e21s1"); }
        }

        public static string e21s10
        {
            get { return ScriptLocalization.Get("Story/e21s10"); }
        }

        public static string e21s11
        {
            get { return ScriptLocalization.Get("Story/e21s11"); }
        }

        public static string e21s12
        {
            get { return ScriptLocalization.Get("Story/e21s12"); }
        }

        public static string e21s13
        {
            get { return ScriptLocalization.Get("Story/e21s13"); }
        }

        public static string e21s14
        {
            get { return ScriptLocalization.Get("Story/e21s14"); }
        }

        public static string e21s15
        {
            get { return ScriptLocalization.Get("Story/e21s15"); }
        }

        public static string e21s16
        {
            get { return ScriptLocalization.Get("Story/e21s16"); }
        }

        public static string e21s17
        {
            get { return ScriptLocalization.Get("Story/e21s17"); }
        }

        public static string e21s18
        {
            get { return ScriptLocalization.Get("Story/e21s18"); }
        }

        public static string e21s19
        {
            get { return ScriptLocalization.Get("Story/e21s19"); }
        }

        public static string e21s2
        {
            get { return ScriptLocalization.Get("Story/e21s2"); }
        }

        public static string e21s20
        {
            get { return ScriptLocalization.Get("Story/e21s20"); }
        }

        public static string e21s21
        {
            get { return ScriptLocalization.Get("Story/e21s21"); }
        }

        public static string e21s22
        {
            get { return ScriptLocalization.Get("Story/e21s22"); }
        }

        public static string e21s23
        {
            get { return ScriptLocalization.Get("Story/e21s23"); }
        }

        public static string e21s24
        {
            get { return ScriptLocalization.Get("Story/e21s24"); }
        }

        public static string e21s25
        {
            get { return ScriptLocalization.Get("Story/e21s25"); }
        }

        public static string e21s26
        {
            get { return ScriptLocalization.Get("Story/e21s26"); }
        }

        public static string e21s27
        {
            get { return ScriptLocalization.Get("Story/e21s27"); }
        }

        public static string e21s28
        {
            get { return ScriptLocalization.Get("Story/e21s28"); }
        }

        public static string e21s29
        {
            get { return ScriptLocalization.Get("Story/e21s29"); }
        }

        public static string e21s3
        {
            get { return ScriptLocalization.Get("Story/e21s3"); }
        }

        public static string e21s30
        {
            get { return ScriptLocalization.Get("Story/e21s30"); }
        }

        public static string e21s4
        {
            get { return ScriptLocalization.Get("Story/e21s4"); }
        }

        public static string e21s6
        {
            get { return ScriptLocalization.Get("Story/e21s6"); }
        }

        public static string e21s7
        {
            get { return ScriptLocalization.Get("Story/e21s7"); }
        }

        public static string e21s8
        {
            get { return ScriptLocalization.Get("Story/e21s8"); }
        }

        public static string e21s9
        {
            get { return ScriptLocalization.Get("Story/e21s9"); }
        }

        public static string e22s1
        {
            get { return ScriptLocalization.Get("Story/e22s1"); }
        }

        public static string e22s2
        {
            get { return ScriptLocalization.Get("Story/e22s2"); }
        }

        public static string e22s3
        {
            get { return ScriptLocalization.Get("Story/e22s3"); }
        }

        public static string e22s4
        {
            get { return ScriptLocalization.Get("Story/e22s4"); }
        }

        public static string e22s5
        {
            get { return ScriptLocalization.Get("Story/e22s5"); }
        }

        public static string e23s1
        {
            get { return ScriptLocalization.Get("Story/e23s1"); }
        }

        public static string e23s10
        {
            get { return ScriptLocalization.Get("Story/e23s10"); }
        }

        public static string e23s11
        {
            get { return ScriptLocalization.Get("Story/e23s11"); }
        }

        public static string e23s12
        {
            get { return ScriptLocalization.Get("Story/e23s12"); }
        }

        public static string e23s13
        {
            get { return ScriptLocalization.Get("Story/e23s13"); }
        }

        public static string e23s14
        {
            get { return ScriptLocalization.Get("Story/e23s14"); }
        }

        public static string e23s15
        {
            get { return ScriptLocalization.Get("Story/e23s15"); }
        }

        public static string e23s16
        {
            get { return ScriptLocalization.Get("Story/e23s16"); }
        }

        public static string e23s17
        {
            get { return ScriptLocalization.Get("Story/e23s17"); }
        }

        public static string e23s18
        {
            get { return ScriptLocalization.Get("Story/e23s18"); }
        }

        public static string e23s2
        {
            get { return ScriptLocalization.Get("Story/e23s2"); }
        }

        public static string e23s3
        {
            get { return ScriptLocalization.Get("Story/e23s3"); }
        }

        public static string e23s4
        {
            get { return ScriptLocalization.Get("Story/e23s4"); }
        }

        public static string e23s8
        {
            get { return ScriptLocalization.Get("Story/e23s8"); }
        }

        public static string e23s9
        {
            get { return ScriptLocalization.Get("Story/e23s9"); }
        }

        public static string e24s1
        {
            get { return ScriptLocalization.Get("Story/e24s1"); }
        }

        public static string e26s1
        {
            get { return ScriptLocalization.Get("Story/e26s1"); }
        }

        public static string e26s10
        {
            get { return ScriptLocalization.Get("Story/e26s10"); }
        }

        public static string e26s11
        {
            get { return ScriptLocalization.Get("Story/e26s11"); }
        }

        public static string e26s2
        {
            get { return ScriptLocalization.Get("Story/e26s2"); }
        }

        public static string e26s3
        {
            get { return ScriptLocalization.Get("Story/e26s3"); }
        }

        public static string e26s4
        {
            get { return ScriptLocalization.Get("Story/e26s4"); }
        }

        public static string e26s5
        {
            get { return ScriptLocalization.Get("Story/e26s5"); }
        }

        public static string e26s6
        {
            get { return ScriptLocalization.Get("Story/e26s6"); }
        }

        public static string e26s7
        {
            get { return ScriptLocalization.Get("Story/e26s7"); }
        }

        public static string e26s8
        {
            get { return ScriptLocalization.Get("Story/e26s8"); }
        }

        public static string e26s9
        {
            get { return ScriptLocalization.Get("Story/e26s9"); }
        }

        public static string e2s1
        {
            get { return ScriptLocalization.Get("Story/e2s1"); }
        }

        public static string e2s2
        {
            get { return ScriptLocalization.Get("Story/e2s2"); }
        }

        public static string e2s3
        {
            get { return ScriptLocalization.Get("Story/e2s3"); }
        }

        public static string e3s1
        {
            get { return ScriptLocalization.Get("Story/e3s1"); }
        }

        public static string e3s2
        {
            get { return ScriptLocalization.Get("Story/e3s2"); }
        }

        public static string e3s3
        {
            get { return ScriptLocalization.Get("Story/e3s3"); }
        }

        public static string e3s4
        {
            get { return ScriptLocalization.Get("Story/e3s4"); }
        }

        public static string e3s5
        {
            get { return ScriptLocalization.Get("Story/e3s5"); }
        }

        public static string e3s6
        {
            get { return ScriptLocalization.Get("Story/e3s6"); }
        }

        public static string e6s1
        {
            get { return ScriptLocalization.Get("Story/e6s1"); }
        }

        public static string e6s10
        {
            get { return ScriptLocalization.Get("Story/e6s10"); }
        }

        public static string e6s11
        {
            get { return ScriptLocalization.Get("Story/e6s11"); }
        }

        public static string e6s12
        {
            get { return ScriptLocalization.Get("Story/e6s12"); }
        }

        public static string e6s13
        {
            get { return ScriptLocalization.Get("Story/e6s13"); }
        }

        public static string e6s14
        {
            get { return ScriptLocalization.Get("Story/e6s14"); }
        }

        public static string e6s15
        {
            get { return ScriptLocalization.Get("Story/e6s15"); }
        }

        public static string e6s2
        {
            get { return ScriptLocalization.Get("Story/e6s2"); }
        }

        public static string e6s3
        {
            get { return ScriptLocalization.Get("Story/e6s3"); }
        }

        public static string e6s4
        {
            get { return ScriptLocalization.Get("Story/e6s4"); }
        }

        public static string e6s5
        {
            get { return ScriptLocalization.Get("Story/e6s5"); }
        }

        public static string e6s6
        {
            get { return ScriptLocalization.Get("Story/e6s6"); }
        }

        public static string e6s7
        {
            get { return ScriptLocalization.Get("Story/e6s7"); }
        }

        public static string e6s8
        {
            get { return ScriptLocalization.Get("Story/e6s8"); }
        }

        public static string e6s9
        {
            get { return ScriptLocalization.Get("Story/e6s9"); }
        }

        public static string e7s1
        {
            get { return ScriptLocalization.Get("Story/e7s1"); }
        }

        public static string e7s2
        {
            get { return ScriptLocalization.Get("Story/e7s2"); }
        }

        public static string e7s3
        {
            get { return ScriptLocalization.Get("Story/e7s3"); }
        }

        public static string e7s4
        {
            get { return ScriptLocalization.Get("Story/e7s4"); }
        }

        public static string e7s5
        {
            get { return ScriptLocalization.Get("Story/e7s5"); }
        }

        public static string e7s6
        {
            get { return ScriptLocalization.Get("Story/e7s6"); }
        }

        public static string e8s1
        {
            get { return ScriptLocalization.Get("Story/e8s1"); }
        }

        public static string e8s2
        {
            get { return ScriptLocalization.Get("Story/e8s2"); }
        }

        public static string e8s3
        {
            get { return ScriptLocalization.Get("Story/e8s3"); }
        }

        public static string e8s4
        {
            get { return ScriptLocalization.Get("Story/e8s4"); }
        }
    }

    public static class ui
    {
        public static string anonymous
        {
            get { return ScriptLocalization.Get("ui/anonymous"); }
        }

        public static string energyMatrixNotEnough
        {
            get { return ScriptLocalization.Get("ui/energyMatrixNotEnough"); }
        }

        public static string ICEY
        {
            get { return ScriptLocalization.Get("ui/ICEY"); }
        }

        public static string UCEY
        {
            get { return ScriptLocalization.Get("ui/UCEY"); }
        }

        public static string UI_font
        {
            get { return ScriptLocalization.Get("ui/UI_font"); }
        }
    }

    public static class ui_bloodPalace
    {
        public static string Addition
        {
            get { return ScriptLocalization.Get("ui/bloodPalace/Addition"); }
        }

        public static string Rank
        {
            get { return ScriptLocalization.Get("ui/bloodPalace/Rank"); }
        }

        public static string Score
        {
            get { return ScriptLocalization.Get("ui/bloodPalace/Score"); }
        }

        public static string Style
        {
            get { return ScriptLocalization.Get("ui/bloodPalace/Style"); }
        }

        public static string Time
        {
            get { return ScriptLocalization.Get("ui/bloodPalace/Time"); }
        }

        public static string TotalScore
        {
            get { return ScriptLocalization.Get("ui/bloodPalace/TotalScore"); }
        }

        public static string Wave
        {
            get { return ScriptLocalization.Get("ui/bloodPalace/Wave"); }
        }
    }

    public static class ui_deadMsg
    {
        public static string again
        {
            get { return ScriptLocalization.Get("ui/deadMsg/again"); }
        }

        public static string restart
        {
            get { return ScriptLocalization.Get("ui/deadMsg/restart"); }
        }
    }

    public static class ui_detailInfo
    {
        public static string _0
        {
            get { return ScriptLocalization.Get("ui/detailInfo/0"); }
        }

        public static string _1
        {
            get { return ScriptLocalization.Get("ui/detailInfo/1"); }
        }

        public static string _2
        {
            get { return ScriptLocalization.Get("ui/detailInfo/2"); }
        }

        public static string _3
        {
            get { return ScriptLocalization.Get("ui/detailInfo/3"); }
        }

        public static string _4
        {
            get { return ScriptLocalization.Get("ui/detailInfo/4"); }
        }

        public static string _5
        {
            get { return ScriptLocalization.Get("ui/detailInfo/5"); }
        }

        public static string _6
        {
            get { return ScriptLocalization.Get("ui/detailInfo/6"); }
        }

        public static string _7
        {
            get { return ScriptLocalization.Get("ui/detailInfo/7"); }
        }

        public static string altitude
        {
            get { return ScriptLocalization.Get("ui/detailInfo/altitude"); }
        }

        public static string coordinate
        {
            get { return ScriptLocalization.Get("ui/detailInfo/coordinate"); }
        }

        public static string meter
        {
            get { return ScriptLocalization.Get("ui/detailInfo/meter"); }
        }

        public static string unknown
        {
            get { return ScriptLocalization.Get("ui/detailInfo/unknown"); }
        }
    }

    public static class ui_detailInfo_coordinate
    {
        public static string _0
        {
            get { return ScriptLocalization.Get("ui/detailInfo/coordinate/0"); }
        }

        public static string _1
        {
            get { return ScriptLocalization.Get("ui/detailInfo/coordinate/1"); }
        }

        public static string _10
        {
            get { return ScriptLocalization.Get("ui/detailInfo/coordinate/10"); }
        }

        public static string _11
        {
            get { return ScriptLocalization.Get("ui/detailInfo/coordinate/11"); }
        }

        public static string _12
        {
            get { return ScriptLocalization.Get("ui/detailInfo/coordinate/12"); }
        }

        public static string _13
        {
            get { return ScriptLocalization.Get("ui/detailInfo/coordinate/13"); }
        }

        public static string _14
        {
            get { return ScriptLocalization.Get("ui/detailInfo/coordinate/14"); }
        }

        public static string _15
        {
            get { return ScriptLocalization.Get("ui/detailInfo/coordinate/15"); }
        }

        public static string _16
        {
            get { return ScriptLocalization.Get("ui/detailInfo/coordinate/16"); }
        }

        public static string _17
        {
            get { return ScriptLocalization.Get("ui/detailInfo/coordinate/17"); }
        }

        public static string _18
        {
            get { return ScriptLocalization.Get("ui/detailInfo/coordinate/18"); }
        }

        public static string _19
        {
            get { return ScriptLocalization.Get("ui/detailInfo/coordinate/19"); }
        }

        public static string _2
        {
            get { return ScriptLocalization.Get("ui/detailInfo/coordinate/2"); }
        }

        public static string _20
        {
            get { return ScriptLocalization.Get("ui/detailInfo/coordinate/20"); }
        }

        public static string _21
        {
            get { return ScriptLocalization.Get("ui/detailInfo/coordinate/21"); }
        }

        public static string _3
        {
            get { return ScriptLocalization.Get("ui/detailInfo/coordinate/3"); }
        }

        public static string _4
        {
            get { return ScriptLocalization.Get("ui/detailInfo/coordinate/4"); }
        }

        public static string _5
        {
            get { return ScriptLocalization.Get("ui/detailInfo/coordinate/5"); }
        }

        public static string _6
        {
            get { return ScriptLocalization.Get("ui/detailInfo/coordinate/6"); }
        }

        public static string _7
        {
            get { return ScriptLocalization.Get("ui/detailInfo/coordinate/7"); }
        }

        public static string _8
        {
            get { return ScriptLocalization.Get("ui/detailInfo/coordinate/8"); }
        }

        public static string _9
        {
            get { return ScriptLocalization.Get("ui/detailInfo/coordinate/9"); }
        }
    }

    public static class ui_endTitle
    {
        public static string falseStuff
        {
            get { return ScriptLocalization.Get("ui/endTitle/falseStuff"); }
        }

        public static string trueStuff
        {
            get { return ScriptLocalization.Get("ui/endTitle/trueStuff"); }
        }
    }

    public static class ui_enhancement
    {
        public static string airAttack
        {
            get { return ScriptLocalization.Get("ui/enhancement/airAttack"); }
        }

        public static string airAttackLv0ControlKeys
        {
            get { return ScriptLocalization.Get("ui/enhancement/airAttackLv0ControlKeys"); }
        }

        public static string airAttackLv0Desc
        {
            get { return ScriptLocalization.Get("ui/enhancement/airAttackLv0Desc"); }
        }

        public static string airAttackLv0NextLevelDesc
        {
            get { return ScriptLocalization.Get("ui/enhancement/airAttackLv0NextLevelDesc"); }
        }

        public static string airAttackLv3NextLevelDesc
        {
            get { return ScriptLocalization.Get("ui/enhancement/airAttackLv3NextLevelDesc"); }
        }

        public static string airAvatarAttack
        {
            get { return ScriptLocalization.Get("ui/enhancement/airAvatarAttack"); }
        }

        public static string airAvatarAttackLv0ControlKeys
        {
            get { return ScriptLocalization.Get("ui/enhancement/airAvatarAttackLv0ControlKeys"); }
        }

        public static string airAvatarAttackLv0Desc
        {
            get { return ScriptLocalization.Get("ui/enhancement/airAvatarAttackLv0Desc"); }
        }

        public static string airAvatarAttackLv0NextLevelDesc
        {
            get { return ScriptLocalization.Get("ui/enhancement/airAvatarAttackLv0NextLevelDesc"); }
        }

        public static string airAvatarAttackLv3NextLevelDesc
        {
            get { return ScriptLocalization.Get("ui/enhancement/airAvatarAttackLv3NextLevelDesc"); }
        }

        public static string airCombo1
        {
            get { return ScriptLocalization.Get("ui/enhancement/airCombo1"); }
        }

        public static string airCombo1Lv0ControlKeys
        {
            get { return ScriptLocalization.Get("ui/enhancement/airCombo1Lv0ControlKeys"); }
        }

        public static string airCombo1Lv0Desc
        {
            get { return ScriptLocalization.Get("ui/enhancement/airCombo1Lv0Desc"); }
        }

        public static string airCombo1Lv0NextLevelDesc
        {
            get { return ScriptLocalization.Get("ui/enhancement/airCombo1Lv0NextLevelDesc"); }
        }

        public static string airCombo1Lv3NextLevelDesc
        {
            get { return ScriptLocalization.Get("ui/enhancement/airCombo1Lv3NextLevelDesc"); }
        }

        public static string airCombo2
        {
            get { return ScriptLocalization.Get("ui/enhancement/airCombo2"); }
        }

        public static string airCombo2Lv0ControlKeys
        {
            get { return ScriptLocalization.Get("ui/enhancement/airCombo2Lv0ControlKeys"); }
        }

        public static string airCombo2Lv0Desc
        {
            get { return ScriptLocalization.Get("ui/enhancement/airCombo2Lv0Desc"); }
        }

        public static string airCombo2Lv0NextLevelDesc
        {
            get { return ScriptLocalization.Get("ui/enhancement/airCombo2Lv0NextLevelDesc"); }
        }

        public static string airCombo2Lv3NextLevelDesc
        {
            get { return ScriptLocalization.Get("ui/enhancement/airCombo2Lv3NextLevelDesc"); }
        }

        public static string attack
        {
            get { return ScriptLocalization.Get("ui/enhancement/attack"); }
        }

        public static string attackLv0ControlKeys
        {
            get { return ScriptLocalization.Get("ui/enhancement/attackLv0ControlKeys"); }
        }

        public static string attackLv0Desc
        {
            get { return ScriptLocalization.Get("ui/enhancement/attackLv0Desc"); }
        }

        public static string attackLv0NextLevelDesc
        {
            get { return ScriptLocalization.Get("ui/enhancement/attackLv0NextLevelDesc"); }
        }

        public static string attackLv3ControlKeys
        {
            get { return ScriptLocalization.Get("ui/enhancement/attackLv3ControlKeys"); }
        }

        public static string attackLv3NextLevelDesc
        {
            get { return ScriptLocalization.Get("ui/enhancement/attackLv3NextLevelDesc"); }
        }

        public static string avatarAttack
        {
            get { return ScriptLocalization.Get("ui/enhancement/avatarAttack"); }
        }

        public static string avatarAttackLv0ControlKeys
        {
            get { return ScriptLocalization.Get("ui/enhancement/avatarAttackLv0ControlKeys"); }
        }

        public static string avatarAttackLv0Desc
        {
            get { return ScriptLocalization.Get("ui/enhancement/avatarAttackLv0Desc"); }
        }

        public static string avatarAttackLv0NextLevelDesc
        {
            get { return ScriptLocalization.Get("ui/enhancement/avatarAttackLv0NextLevelDesc"); }
        }

        public static string avatarAttackLv3NextLevelDesc
        {
            get { return ScriptLocalization.Get("ui/enhancement/avatarAttackLv3NextLevelDesc"); }
        }

        public static string bladeStorm
        {
            get { return ScriptLocalization.Get("ui/enhancement/bladeStorm"); }
        }

        public static string bladeStormLv0ControlKeys
        {
            get { return ScriptLocalization.Get("ui/enhancement/bladeStormLv0ControlKeys"); }
        }

        public static string bladeStormLv0Desc
        {
            get { return ScriptLocalization.Get("ui/enhancement/bladeStormLv0Desc"); }
        }

        public static string bladeStormLv0NextLevelDesc
        {
            get { return ScriptLocalization.Get("ui/enhancement/bladeStormLv0NextLevelDesc"); }
        }

        public static string bladeStormLv3NextLevelDesc
        {
            get { return ScriptLocalization.Get("ui/enhancement/bladeStormLv3NextLevelDesc"); }
        }

        public static string charging
        {
            get { return ScriptLocalization.Get("ui/enhancement/charging"); }
        }

        public static string chargingLv0ControlKeys
        {
            get { return ScriptLocalization.Get("ui/enhancement/chargingLv0ControlKeys"); }
        }

        public static string chargingLv0Desc
        {
            get { return ScriptLocalization.Get("ui/enhancement/chargingLv0Desc"); }
        }

        public static string chargingLv0NextLevelDesc
        {
            get { return ScriptLocalization.Get("ui/enhancement/chargingLv0NextLevelDesc"); }
        }

        public static string chargingLv3NextLevelDesc
        {
            get { return ScriptLocalization.Get("ui/enhancement/chargingLv3NextLevelDesc"); }
        }

        public static string chase
        {
            get { return ScriptLocalization.Get("ui/enhancement/chase"); }
        }

        public static string chaseLv0ControlKeys
        {
            get { return ScriptLocalization.Get("ui/enhancement/chaseLv0ControlKeys"); }
        }

        public static string chaseLv0Desc
        {
            get { return ScriptLocalization.Get("ui/enhancement/chaseLv0Desc"); }
        }

        public static string chaseLv0NextLevelDesc
        {
            get { return ScriptLocalization.Get("ui/enhancement/chaseLv0NextLevelDesc"); }
        }

        public static string chaseLv2NextLevelDesc
        {
            get { return ScriptLocalization.Get("ui/enhancement/chaseLv2NextLevelDesc"); }
        }

        public static string chaseLv3NextLevelDesc
        {
            get { return ScriptLocalization.Get("ui/enhancement/chaseLv3NextLevelDesc"); }
        }

        public static string combo1
        {
            get { return ScriptLocalization.Get("ui/enhancement/combo1"); }
        }

        public static string combo1Lv0ControlKeys
        {
            get { return ScriptLocalization.Get("ui/enhancement/combo1Lv0ControlKeys"); }
        }

        public static string combo1Lv0Desc
        {
            get { return ScriptLocalization.Get("ui/enhancement/combo1Lv0Desc"); }
        }

        public static string combo1Lv0NextLevelDesc
        {
            get { return ScriptLocalization.Get("ui/enhancement/combo1Lv0NextLevelDesc"); }
        }

        public static string combo1Lv3NextLevelDesc
        {
            get { return ScriptLocalization.Get("ui/enhancement/combo1Lv3NextLevelDesc"); }
        }

        public static string combo2
        {
            get { return ScriptLocalization.Get("ui/enhancement/combo2"); }
        }

        public static string combo2Lv0ControlKeys
        {
            get { return ScriptLocalization.Get("ui/enhancement/combo2Lv0ControlKeys"); }
        }

        public static string combo2Lv0Desc
        {
            get { return ScriptLocalization.Get("ui/enhancement/combo2Lv0Desc"); }
        }

        public static string combo2Lv0NextLevelDesc
        {
            get { return ScriptLocalization.Get("ui/enhancement/combo2Lv0NextLevelDesc"); }
        }

        public static string combo2Lv3NextLevelDesc
        {
            get { return ScriptLocalization.Get("ui/enhancement/combo2Lv3NextLevelDesc"); }
        }

        public static string exit
        {
            get { return ScriptLocalization.Get("ui/enhancement/exit"); }
        }

        public static string flashAttack
        {
            get { return ScriptLocalization.Get("ui/enhancement/flashAttack"); }
        }

        public static string flashAttackLv0ControlKeys
        {
            get { return ScriptLocalization.Get("ui/enhancement/flashAttackLv0ControlKeys"); }
        }

        public static string flashAttackLv0Desc
        {
            get { return ScriptLocalization.Get("ui/enhancement/flashAttackLv0Desc"); }
        }

        public static string flashAttackLv0NextLevelDesc
        {
            get { return ScriptLocalization.Get("ui/enhancement/flashAttackLv0NextLevelDesc"); }
        }

        public static string flashAttackLv3NextLevelDesc
        {
            get { return ScriptLocalization.Get("ui/enhancement/flashAttackLv3NextLevelDesc"); }
        }

        public static string hitGround
        {
            get { return ScriptLocalization.Get("ui/enhancement/hitGround"); }
        }

        public static string hitGroundLv0ControlKeys
        {
            get { return ScriptLocalization.Get("ui/enhancement/hitGroundLv0ControlKeys"); }
        }

        public static string hitGroundLv0Desc
        {
            get { return ScriptLocalization.Get("ui/enhancement/hitGroundLv0Desc"); }
        }

        public static string hitGroundLv0NextLevelDesc
        {
            get { return ScriptLocalization.Get("ui/enhancement/hitGroundLv0NextLevelDesc"); }
        }

        public static string hitGroundLv3NextLevelDesc
        {
            get { return ScriptLocalization.Get("ui/enhancement/hitGroundLv3NextLevelDesc"); }
        }

        public static string knockout
        {
            get { return ScriptLocalization.Get("ui/enhancement/knockout"); }
        }

        public static string knockoutLv0ControlKeys
        {
            get { return ScriptLocalization.Get("ui/enhancement/knockoutLv0ControlKeys"); }
        }

        public static string knockoutLv0Desc
        {
            get { return ScriptLocalization.Get("ui/enhancement/knockoutLv0Desc"); }
        }

        public static string knockoutLv0NextLevelDesc
        {
            get { return ScriptLocalization.Get("ui/enhancement/knockoutLv0NextLevelDesc"); }
        }

        public static string knockoutLv3NextLevelDesc
        {
            get { return ScriptLocalization.Get("ui/enhancement/knockoutLv3NextLevelDesc"); }
        }

        public static string levelUp
        {
            get { return ScriptLocalization.Get("ui/enhancement/levelUp"); }
        }

        public static string maxEnergy
        {
            get { return ScriptLocalization.Get("ui/enhancement/maxEnergy"); }
        }

        public static string maxEnergyLv0Desc
        {
            get { return ScriptLocalization.Get("ui/enhancement/maxEnergyLv0Desc"); }
        }

        public static string maxEnergyLv0NextLevelDesc
        {
            get { return ScriptLocalization.Get("ui/enhancement/maxEnergyLv0NextLevelDesc"); }
        }

        public static string maxEnergyLv3NextLevelDesc
        {
            get { return ScriptLocalization.Get("ui/enhancement/maxEnergyLv3NextLevelDesc"); }
        }

        public static string maxEnergyMatrix
        {
            get { return ScriptLocalization.Get("ui/enhancement/maxEnergyMatrix"); }
        }

        public static string maxHP
        {
            get { return ScriptLocalization.Get("ui/enhancement/maxHP"); }
        }

        public static string maxHPLv0Desc
        {
            get { return ScriptLocalization.Get("ui/enhancement/maxHPLv0Desc"); }
        }

        public static string maxHPLv0NextLevelDesc
        {
            get { return ScriptLocalization.Get("ui/enhancement/maxHPLv0NextLevelDesc"); }
        }

        public static string maxHPLv3NextLevelDesc
        {
            get { return ScriptLocalization.Get("ui/enhancement/maxHPLv3NextLevelDesc"); }
        }

        public static string recover
        {
            get { return ScriptLocalization.Get("ui/enhancement/recover"); }
        }

        public static string recoverLv0Desc
        {
            get { return ScriptLocalization.Get("ui/enhancement/recoverLv0Desc"); }
        }

        public static string recoverLv0NextLevelDesc
        {
            get { return ScriptLocalization.Get("ui/enhancement/recoverLv0NextLevelDesc"); }
        }

        public static string recoverLv3NextLevelDesc
        {
            get { return ScriptLocalization.Get("ui/enhancement/recoverLv3NextLevelDesc"); }
        }

        public static string shadeAttack
        {
            get { return ScriptLocalization.Get("ui/enhancement/shadeAttack"); }
        }

        public static string shadeAttackLv0ControlKeys
        {
            get { return ScriptLocalization.Get("ui/enhancement/shadeAttackLv0ControlKeys"); }
        }

        public static string shadeAttackLv0Desc
        {
            get { return ScriptLocalization.Get("ui/enhancement/shadeAttackLv0Desc"); }
        }

        public static string shadeAttackLv0NextLevelDesc
        {
            get { return ScriptLocalization.Get("ui/enhancement/shadeAttackLv0NextLevelDesc"); }
        }

        public static string shadeAttackLv3NextLevelDesc
        {
            get { return ScriptLocalization.Get("ui/enhancement/shadeAttackLv3NextLevelDesc"); }
        }

        public static string tripleAttack
        {
            get { return ScriptLocalization.Get("ui/enhancement/tripleAttack"); }
        }

        public static string tripleAttackLv0ControlKeys
        {
            get { return ScriptLocalization.Get("ui/enhancement/tripleAttackLv0ControlKeys"); }
        }

        public static string tripleAttackLv0Desc
        {
            get { return ScriptLocalization.Get("ui/enhancement/tripleAttackLv0Desc"); }
        }

        public static string tripleAttackLv0NextLevelDesc
        {
            get { return ScriptLocalization.Get("ui/enhancement/tripleAttackLv0NextLevelDesc"); }
        }

        public static string tripleAttackLv3NextLevelDesc
        {
            get { return ScriptLocalization.Get("ui/enhancement/tripleAttackLv3NextLevelDesc"); }
        }

        public static string turnOff
        {
            get { return ScriptLocalization.Get("ui/enhancement/turnOff"); }
        }

        public static string upperChop
        {
            get { return ScriptLocalization.Get("ui/enhancement/upperChop"); }
        }

        public static string upperChopLv0ControlKeys
        {
            get { return ScriptLocalization.Get("ui/enhancement/upperChopLv0ControlKeys"); }
        }

        public static string upperChopLv0Desc
        {
            get { return ScriptLocalization.Get("ui/enhancement/upperChopLv0Desc"); }
        }

        public static string upperChopLv0NextLevelDesc
        {
            get { return ScriptLocalization.Get("ui/enhancement/upperChopLv0NextLevelDesc"); }
        }

        public static string upperChopLv3NextLevelDesc
        {
            get { return ScriptLocalization.Get("ui/enhancement/upperChopLv3NextLevelDesc"); }
        }
    }

    public static class ui_hitsGrade
    {
        public static string damage
        {
            get { return ScriptLocalization.Get("ui/hitsGrade/damage"); }
        }

        public static string hits
        {
            get { return ScriptLocalization.Get("ui/hitsGrade/hits"); }
        }
    }

    public static class ui_key
    {
        public static string action
        {
            get { return ScriptLocalization.Get("ui/key/action"); }
        }

        public static string flash
        {
            get { return ScriptLocalization.Get("ui/key/flash"); }
        }

        public static string function1
        {
            get { return ScriptLocalization.Get("ui/key/function1"); }
        }

        public static string function2
        {
            get { return ScriptLocalization.Get("ui/key/function2"); }
        }

        public static string HeavyAttack
        {
            get { return ScriptLocalization.Get("ui/key/HeavyAttack"); }
        }

        public static string jump
        {
            get { return ScriptLocalization.Get("ui/key/jump"); }
        }

        public static string Keypad
        {
            get { return ScriptLocalization.Get("ui/key/Keypad"); }
        }

        public static string Left
        {
            get { return ScriptLocalization.Get("ui/key/Left"); }
        }

        public static string moveDown
        {
            get { return ScriptLocalization.Get("ui/key/moveDown"); }
        }

        public static string moveLeft
        {
            get { return ScriptLocalization.Get("ui/key/moveLeft"); }
        }

        public static string moveRight
        {
            get { return ScriptLocalization.Get("ui/key/moveRight"); }
        }

        public static string moveUp
        {
            get { return ScriptLocalization.Get("ui/key/moveUp"); }
        }

        public static string normalAttack
        {
            get { return ScriptLocalization.Get("ui/key/normalAttack"); }
        }

        public static string option1
        {
            get { return ScriptLocalization.Get("ui/key/option1"); }
        }

        public static string option2
        {
            get { return ScriptLocalization.Get("ui/key/option2"); }
        }

        public static string Reset
        {
            get { return ScriptLocalization.Get("ui/key/Reset"); }
        }

        public static string Right
        {
            get { return ScriptLocalization.Get("ui/key/Right"); }
        }

        public static string skill1
        {
            get { return ScriptLocalization.Get("ui/key/skill1"); }
        }

        public static string skill2
        {
            get { return ScriptLocalization.Get("ui/key/skill2"); }
        }
    }

    public static class ui_language
    {
        public static string english
        {
            get { return ScriptLocalization.Get("ui/language/english"); }
        }

        public static string french
        {
            get { return ScriptLocalization.Get("ui/language/french"); }
        }

        public static string japanese
        {
            get { return ScriptLocalization.Get("ui/language/japanese"); }
        }

        public static string mandarin_chinese
        {
            get { return ScriptLocalization.Get("ui/language/mandarin_chinese"); }
        }

        public static string spanish
        {
            get { return ScriptLocalization.Get("ui/language/spanish"); }
        }
    }

    public static class ui_levelName
    {
        public static string C1L1S1
        {
            get { return ScriptLocalization.Get("ui/levelName/C1L1S1"); }
        }

        public static string C1L1S7
        {
            get { return ScriptLocalization.Get("ui/levelName/C1L1S7"); }
        }

        public static string C1L1S8
        {
            get { return ScriptLocalization.Get("ui/levelName/C1L1S8"); }
        }

        public static string C1L2S1
        {
            get { return ScriptLocalization.Get("ui/levelName/C1L2S1"); }
        }

        public static string C1L2S6
        {
            get { return ScriptLocalization.Get("ui/levelName/C1L2S6"); }
        }

        public static string C1L3S1
        {
            get { return ScriptLocalization.Get("ui/levelName/C1L3S1"); }
        }

        public static string C1L3S15
        {
            get { return ScriptLocalization.Get("ui/levelName/C1L3S15"); }
        }

        public static string C1L3S16
        {
            get { return ScriptLocalization.Get("ui/levelName/C1L3S16"); }
        }

        public static string C2L1S2
        {
            get { return ScriptLocalization.Get("ui/levelName/C2L1S2"); }
        }

        public static string C2L2S1
        {
            get { return ScriptLocalization.Get("ui/levelName/C2L2S1"); }
        }

        public static string C3L1S1
        {
            get { return ScriptLocalization.Get("ui/levelName/C3L1S1"); }
        }

        public static string C3L1S4
        {
            get { return ScriptLocalization.Get("ui/levelName/C3L1S4"); }
        }

        public static string C4L1S15
        {
            get { return ScriptLocalization.Get("ui/levelName/C4L1S15"); }
        }

        public static string C4L1S4
        {
            get { return ScriptLocalization.Get("ui/levelName/C4L1S4"); }
        }

        public static string C4L1S7
        {
            get { return ScriptLocalization.Get("ui/levelName/C4L1S7"); }
        }

        public static string C4L2S1
        {
            get { return ScriptLocalization.Get("ui/levelName/C4L2S1"); }
        }

        public static string C5L1S1
        {
            get { return ScriptLocalization.Get("ui/levelName/C5L1S1"); }
        }

        public static string C6L1S1
        {
            get { return ScriptLocalization.Get("ui/levelName/C6L1S1"); }
        }
    }

    public static class ui_levelSelect
    {
        public static string about_content
        {
            get { return ScriptLocalization.Get("ui/levelSelect/about_content"); }
        }

        public static string about_i_txt
        {
            get { return ScriptLocalization.Get("ui/levelSelect/about_i_txt"); }
        }

        public static string icey_bin
        {
            get { return ScriptLocalization.Get("ui/levelSelect/icey_bin"); }
        }

        public static string key
        {
            get { return ScriptLocalization.Get("ui/levelSelect/key"); }
        }

        public static string key_desc
        {
            get { return ScriptLocalization.Get("ui/levelSelect/key_desc"); }
        }

        public static string password
        {
            get { return ScriptLocalization.Get("ui/levelSelect/password"); }
        }

        public static string password_desc
        {
            get { return ScriptLocalization.Get("ui/levelSelect/password_desc"); }
        }

        public static string readme_content
        {
            get { return ScriptLocalization.Get("ui/levelSelect/readme_content"); }
        }

        public static string readme_txt
        {
            get { return ScriptLocalization.Get("ui/levelSelect/readme_txt"); }
        }
    }

    public static class ui_pause
    {
        public static string exit
        {
            get { return ScriptLocalization.Get("ui/pause/exit"); }
        }

        public static string levelSelect
        {
            get { return ScriptLocalization.Get("ui/pause/levelSelect"); }
        }

        public static string resume
        {
            get { return ScriptLocalization.Get("ui/pause/resume"); }
        }

        public static string warning
        {
            get { return ScriptLocalization.Get("ui/pause/warning"); }
        }
    }

    public static class ui_phone
    {
        public static string accept
        {
            get { return ScriptLocalization.Get("ui/phone/accept"); }
        }

        public static string decline
        {
            get { return ScriptLocalization.Get("ui/phone/decline"); }
        }
    }

    public static class ui_start
    {
        public static string _continue
        {
            get { return ScriptLocalization.Get("ui/start/_continue"); }
        }

        public static string audio_effect
        {
            get { return ScriptLocalization.Get("ui/start/audio_effect"); }
        }

        public static string audio_option
        {
            get { return ScriptLocalization.Get("ui/start/audio_option"); }
        }

        public static string BGM
        {
            get { return ScriptLocalization.Get("ui/start/BGM"); }
        }

        public static string cancel
        {
            get { return ScriptLocalization.Get("ui/start/cancel"); }
        }

        public static string confirm
        {
            get { return ScriptLocalization.Get("ui/start/confirm"); }
        }

        public static string confirm_to_delete_all_data
        {
            get { return ScriptLocalization.Get("ui/start/confirm_to_delete_all_data"); }
        }

        public static string control_option
        {
            get { return ScriptLocalization.Get("ui/start/control_option"); }
        }

        public static string controller_display
        {
            get { return ScriptLocalization.Get("ui/start/controller_display"); }
        }

        public static string delete_all_data
        {
            get { return ScriptLocalization.Get("ui/start/delete_all_data"); }
        }

        public static string exist_duplicate_key
        {
            get { return ScriptLocalization.Get("ui/start/exist_duplicate_key"); }
        }

        public static string exit
        {
            get { return ScriptLocalization.Get("ui/start/exit"); }
        }

        public static string full_screen
        {
            get { return ScriptLocalization.Get("ui/start/full_screen"); }
        }

        public static string graphic_option
        {
            get { return ScriptLocalization.Get("ui/start/graphic_option"); }
        }

        public static string half
        {
            get { return ScriptLocalization.Get("ui/start/half"); }
        }

        public static string key_setting
        {
            get { return ScriptLocalization.Get("ui/start/key_setting"); }
        }

        public static string keyboard
        {
            get { return ScriptLocalization.Get("ui/start/keyboard"); }
        }

        public static string language
        {
            get { return ScriptLocalization.Get("ui/start/language"); }
        }

        public static string off
        {
            get { return ScriptLocalization.Get("ui/start/off"); }
        }

        public static string on
        {
            get { return ScriptLocalization.Get("ui/start/on"); }
        }

        public static string option
        {
            get { return ScriptLocalization.Get("ui/start/option"); }
        }

        public static string press_key_to_start
        {
            get { return ScriptLocalization.Get("ui/start/press_key_to_start"); }
        }

        public static string resolution
        {
            get { return ScriptLocalization.Get("ui/start/resolution"); }
        }

        public static string start
        {
            get { return ScriptLocalization.Get("ui/start/start"); }
        }

        public static string start_with_voiceover
        {
            get { return ScriptLocalization.Get("ui/start/start_with_voiceover"); }
        }

        public static string vibration
        {
            get { return ScriptLocalization.Get("ui/start/vibration"); }
        }

        public static string voiceover
        {
            get { return ScriptLocalization.Get("ui/start/voiceover"); }
        }

        public static string vsync
        {
            get { return ScriptLocalization.Get("ui/start/vsync"); }
        }

        public static string wait_for_input
        {
            get { return ScriptLocalization.Get("ui/start/wait_for_input"); }
        }
    }

    public static class ui_tutorial
    {
        public static string armor
        {
            get { return ScriptLocalization.Get("ui/tutorial/armor"); }
        }

        public static string attack
        {
            get { return ScriptLocalization.Get("ui/tutorial/attack"); }
        }

        public static string back2I
        {
            get { return ScriptLocalization.Get("ui/tutorial/back2I"); }
        }

        public static string breakAmmor
        {
            get { return ScriptLocalization.Get("ui/tutorial/breakAmmor"); }
        }

        public static string charging
        {
            get { return ScriptLocalization.Get("ui/tutorial/charging"); }
        }

        public static string excute
        {
            get { return ScriptLocalization.Get("ui/tutorial/excute"); }
        }

        public static string flash
        {
            get { return ScriptLocalization.Get("ui/tutorial/flash"); }
        }

        public static string flashAttack
        {
            get { return ScriptLocalization.Get("ui/tutorial/flashAttack"); }
        }

        public static string heavyAttack
        {
            get { return ScriptLocalization.Get("ui/tutorial/heavyAttack"); }
        }

        public static string jump
        {
            get { return ScriptLocalization.Get("ui/tutorial/jump"); }
        }

        public static string jumpAcross
        {
            get { return ScriptLocalization.Get("ui/tutorial/jumpAcross"); }
        }

        public static string moreComboMoreAttack
        {
            get { return ScriptLocalization.Get("ui/tutorial/moreComboMoreAttack"); }
        }

        public static string moreCrytalMoreAttack
        {
            get { return ScriptLocalization.Get("ui/tutorial/moreCrytalMoreAttack"); }
        }

        public static string move
        {
            get { return ScriptLocalization.Get("ui/tutorial/move"); }
        }

        public static string ukemi
        {
            get { return ScriptLocalization.Get("ui/tutorial/ukemi"); }
        }

        public static string warning
        {
            get { return ScriptLocalization.Get("ui/tutorial/warning"); }
        }
    }
}