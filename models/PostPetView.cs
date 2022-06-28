using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace mascotas.Models
{
    public class PostPetView
    {
        public int id { get; set; }
        public string userName { get; set; }
        public string petName { get; set; }
        public string petState { get; set; }
        public string petSpecie { get; set; }
        public string? petBreed { get; set; }
        public string provinciaName { get; set; }
        public string cantonName { get; set; }
        public string? sectorName { get; set; }
        public string description { get; set; }
        public decimal? reward { get; set; }
        public DateTime lastTimeSeen { get; set; }
        public string? linkMapSeen { get; set; }
        public List<imgModel> imgs { get; set; }
    }

    public class CreatePostPetDTO
    {
        [Required]
        public int idUser { get; set; }
        [Required]
        public string petName { get; set; }
        [Required]
        public string idState { get; set; }
        [Required]
        public int idPetSpecie { get; set; }
        public int? idPetBreed { get; set; }
        [Required]
        public int idProvincia { get; set; }
        [Required]
        public int idCanton { get; set; }
        public int? idSector { get; set; }
        [Required]
        public string description { get; set; }
        public decimal? reward { get; set; }
        [Required]
        public DateTime lastTimeSeen { get; set; }
        public string? linkMapSeen { get; set; }
        [Required]
        public imgModel[] urlImgs { get; set; }
    }

    public class UpdatePostPetDTO
    {
        [Required]
        public int IdPostPet { get; set; }
        public int IdUser { get; set; }
        public string? PetName { get; set; }
        public string? IdState { get; set; }
        public int? IdPetSpecie { get; set; }
        public int? IdPetBreed { get; set; }
        public int? IdProvincia { get; set; }
        public int? IdCanton { get; set; }
        public int? IdSector { get; set; }
        public string? Description { get; set; }
        public decimal? Reward { get; set; }
        public DateTime? LastTimeSeen { get; set; }
        public string? LinkMapSeen { get; set; }
        public updatePostImgDTO[]? Imgs { get; set; }
    }
}