﻿using System.Collections.Generic;
using System.Text;
using RimWorld;
using UnityEngine;
using Verse;

namespace SR.DA.Thing
{
    public class Building_BondageBed : Building_Bed
    {
        public Pawn occupant;//使用者
        /// <summary>
        /// 获取睡眠位置
        /// </summary>
        /// <returns></returns>
        private IntVec3 GetSleepingSlotPos()
        {
            CellRect cellRect = GenAdj.OccupiedRect(Position, Rotation, def.size);
            return new IntVec3(cellRect.minX, Position.y, cellRect.minZ);
        }
        /// <summary>
        /// 绘制囚犯房间边缘
        /// </summary>
        public override void DrawExtraSelectionOverlays()
        {
            base.DrawExtraSelectionOverlays();
        }
        /// <summary>
        /// 绘制命令
        /// </summary>
        /// <returns></returns>
        public override IEnumerable<Gizmo> GetGizmos()
        {
            yield break;
        }
        /// <summary>
        /// label绘制
        /// </summary>
        public override void DrawGUIOverlay()
        {
            if (occupant != null)
            {
                Color defaultThingLabelColor = Color.yellow;
                GenMapUI.DrawThingLabel(this, "SR_Bound".Translate(), defaultThingLabelColor);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string GetInspectString()
        {
            StringBuilder stringBuilder = new StringBuilder();
			if (this.def.building.bed_humanlike)
			{
                stringBuilder.AppendInNewLine("ForPrisonerUse".Translate());
			}
			return stringBuilder.ToString();
        }
        /// <summary>
        /// 安装
        /// </summary>
        /// <param name="map"></param>
        /// <param name="respawningAfterLoad"></param>
        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            base.SpawnSetup(map, respawningAfterLoad);
        }
        /// <summary>
        /// 拆卸
        /// </summary>
        /// <param name="mode"></param>
        public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
        {
            RemoveOccupant();
            Room room = this.GetRoom(RegionType.Set_Passable);
            base.DeSpawn(mode);
            if (room != null)
            {
                room.Notify_RoomShapeOrContainedBedsChanged();
            }
        }
        /// <summary>
        /// 设置使用者
        /// </summary>
        /// <param name="occupant"></param>
        public void SetOccupant(Pawn occupant) {
            this.occupant = occupant;
            OwnersForReading.Clear();
            OwnersForReading.Add(occupant);
            occupant.jobs.Notify_TuckedIntoBed(this);
        }
        /// <summary>
        /// 移除使用者
        /// </summary>
        public void RemoveOccupant() {
            occupant = null;
            OwnersForReading.Clear();
        }
        /// <summary>
        /// 绘制选项
        /// </summary>
        /// <param name="myPawn"></param>
        /// <returns></returns>
        public override IEnumerable<FloatMenuOption> GetFloatMenuOptions(Pawn myPawn)
        {
            if (AllComps != null)
            {
                for (int i = 0; i < AllComps.Count; i++)
                {
                    foreach (FloatMenuOption floatMenuOption in AllComps[i].CompFloatMenuOptions(myPawn))
                    {
                        yield return floatMenuOption;
                    }
                }
            }
        }
    }
}