using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TempoDigitalApex3.Models
{
    public class ChannelData
    {

        string ChannelId;
        string ChannelName;
        int views;
        double revenue;

        string top1;
        string top2;
        string top3;
        string top4;
        string top5;

        int top1V;
        int top2V;
        int top3V;
        int top4V;
        int top5V;

        int month;
        int year;

        public string ChannelId1 { get => ChannelId; set => ChannelId = value; }
        public string ChannelName1 { get => ChannelName; set => ChannelName = value; }
        public int Views { get => views; set => views = value; }
        public double Revenue { get => revenue; set => revenue = value; }
        public string Top1 { get => top1; set => top1 = value; }
        public string Top2 { get => top2; set => top2 = value; }
        public string Top3 { get => top3; set => top3 = value; }
        public string Top4 { get => top4; set => top4 = value; }
        public string Top5 { get => top5; set => top5 = value; }
        public int Top1V { get => top1V; set => top1V = value; }
        public int Top2V { get => top2V; set => top2V = value; }
        public int Top3V { get => top3V; set => top3V = value; }
        public int Top4V { get => top4V; set => top4V = value; }
        public int Top5V { get => top5V; set => top5V = value; }
        public int Month { get => month; set => month = value; }
        public int Year { get => year; set => year = value; }
    }
}