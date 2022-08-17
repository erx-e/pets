using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace mascotas.Models
{
    public class PostPetView
    {
        public int id { get; set; }
        public string userName { get; set; }
        public int idUser { get; set; }
        public string petName { get; set; }
        public string petAge { get; set; }
        public string? petSpecialCondition { get; set; }
        public string contact { get; set; }
        public string petState { get; set; }
        public string petStateId { get; set; }
        public string petSpecie { get; set; }
        public string? petBreed { get; set; }
        public string provinciaName { get; set; }
        public string cantonName { get; set; }
        public string? sectorName { get; set; }
        public string description { get; set; }
        public decimal? reward { get; set; }
        public DateTime lastTimeSeen { get; set; }
        public string? linkMapSeen { get; set; }
        public List<imgModel> urlImgs { get; set; }
    }

    public class CreatePostPetDTO
    {
        [Required]
        public int idUser { get; set; }
        public string? petName { get; set; }
        public string? petAge { get; set; }
        public string? petSpecialCondition { get; set; }
        public string? contact { get; set; }
        [Required]
        public string idState { get; set; }
        [Required]
        public int idPetSpecie { get; set; }
        [Required]
        public int idPetBreed { get; set; }
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
        public string[] urlImgs { get; set; }
    }

    public class UpdatePostPetDTO
    {
        [Required]
        public int idPostPet { get; set; }
        public int idUser { get; set; }
        public string? petName { get; set; }
        public string? petAge { get; set; }
        public string? petSpecialCondition { get; set; }
        public string? contact { get; set; }
        public string? idState { get; set; }
        public int? idPetSpecie { get; set; }
        public int? idPetBreed { get; set; }
        public int? idProvincia { get; set; }
        public int? idCanton { get; set; }
        public int? idSector { get; set; }
        public string? description { get; set; }
        public decimal? reward { get; set; }
        public DateTime? lastTimeSeen { get; set; }
        public string? linkMapSeen { get; set; }
        public List<updatePostImgDTO>? urlImgs { get; set; }
    }
}