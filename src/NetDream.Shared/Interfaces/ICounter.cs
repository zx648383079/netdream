using NetDream.Shared.Models;
using System;
using System.Collections.Generic;

namespace NetDream.Shared.Interfaces
{
    public interface ICounter
    {

        public int Count(DateOnly date);
        public int Count(int year);
        public int Count(int year, int month);
        public int Count(int year, int month, int day);
        public int Count(int year, int month, int day, int hour);
        public int Count(int year, int month, int day, int hour, int minute);

        public int Count(DateTime startAt);
        public int Count(DateTime startAt, DateTime endAt);


        public int Count(ModuleTargetType type);
        public int Count(ModuleTargetType type, DateOnly date);
        public int Count(ModuleTargetType type, int year);
        public int Count(ModuleTargetType type, int year, int month);
        public int Count(ModuleTargetType type, int year, int month, int day);
        public int Count(ModuleTargetType type, int year, int month, int day, int hour);
        public int Count(ModuleTargetType type, int year, int month, int day, int hour, int minute);

        public int Count(ModuleTargetType type, DateTime startAt);
        public int Count(ModuleTargetType type, DateTime startAt, DateTime endAt);


        public int Count(ModuleTargetType type, int target, DateOnly date);
        public int Count(ModuleTargetType type, int target, DateTime startAt);
        public int Count(ModuleTargetType type, int target, DateTime startAt, DateTime endAt);
        public void Add(ModuleTargetType type, int target);
        /// <summary>
        /// 按周期统计，并排序，从多到少
        /// </summary>
        /// <param name="type"></param>
        /// <param name="periodType"></param>
        /// <returns>(target, count)</returns>
        public KeyValuePair<int, int>[] Count(ModuleTargetType type, StatisticalPeriodType periodType);
        public KeyValuePair<int, int>[] Count(ModuleTargetType type, StatisticalPeriodType periodType, DateTime startAt);
    }
}
