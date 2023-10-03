﻿using System.ComponentModel.DataAnnotations;
namespace HabitAqui_Software.Models
{
    public class DeliveryStatus
    {
        public int Id { get; set; }

        [Display(Name = "hasEquipments", Prompt = "check if habitation has an extra equipments...")]
        public Boolean hasEquipments;

        [Display(Name = "hasDamage", Prompt = "check if habitation has any kind of damage...")]
        public Boolean hasDamage;

        [Display(Name = "Observation", Prompt = "Write any observation about delivery status to client...")]
        public string? observation;

    }
}