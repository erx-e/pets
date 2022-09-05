using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using mascotas.validations;

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
        [Required(ErrorMessage = "Must send {0}")]
        public int idUser { get; set; }

        [MaxLength(45, ErrorMessage = "The {0} can not have more than {1} characters")]
        public string? petName { get; set; }

        [MaxLength(20, ErrorMessage = "The {0} can not have more than {1} characters")]
        public string? petAge { get; set; }

        public string? petSpecialCondition { get; set; }

        [MaxLength(45, ErrorMessage = "The {0} can not have more than {1} characters")]
        [RegularExpression(@"^([0-9]{10}\s{1}){1,3}([0-9]{10}\s*)?$",
        ErrorMessage = "must send cell numbers separated by spaces using the regex {1}")]
        public string? contact { get; set; }

        [Required(ErrorMessage = "Must send property {0}"),]
        public string idState { get; set; }

        [Required(ErrorMessage = "Must send pet specie id property {0}"),]
        public int idPetSpecie { get; set; }

        [Required(ErrorMessage = "Must send pet breed id property {0}"),]
        public int idPetBreed { get; set; }

        [Required(ErrorMessage = "Must send provincia id property {0}"),]
        public int idProvincia { get; set; }

        [Required(ErrorMessage = "Must send canton id property {0}"),]
        public int idCanton { get; set; }

        public int? idSector { get; set; }

        [Required(ErrorMessage = "Must send a description property {0}"),]
        public string description { get; set; }

        [Range(0, 100000, ErrorMessage = "The {0} should be between {1} and {2} dollars")]
        public decimal? reward { get; set; }

        [DataType(DataType.DateTime)]
        [Required(ErrorMessage = "Must send lastTimeSeen property {0}"),]
        [DateRange(ErrorMessage = "Value for {0} must be between {1} and {2}")]
        public DateTime lastTimeSeen { get; set; }

        public string? linkMapSeen { get; set; }

        [Required(ErrorMessage = "Must send an array of the img's url"),]
        [MaxLength(6, ErrorMessage = "{0} must have a max of 6 urls")]
        public imgModel[] urlImgs { get; set; }
    }

    public class UpdatePostPetDTO
    {
        [Required(ErrorMessage = "Must send {0}")]
        public int idPostPet { get; set; }

        [Required(ErrorMessage = "Must send {0}")]
        public int idUser { get; set; }

        [MaxLength(45, ErrorMessage = "The {0} can not have more than {1} characters")]
        public string? petName { get; set; }

        [MaxLength(20, ErrorMessage = "The {0} can not have more than {1} characters")]
        public string? petAge { get; set; }

        public string? petSpecialCondition { get; set; }

        [MaxLength(45, ErrorMessage = "The {0} can not have more than {1} characters")]
        [RegularExpression(@"^([0-9]{10}\s{1}){1,3}([0-9]{10}\s*)?$",
        ErrorMessage = "must send cell numbers separated by spaces using the regex {1}")]
        public string? contact { get; set; }

        public string? idState { get; set; }
        public int? idPetSpecie { get; set; }
        public int? idPetBreed { get; set; }
        public int? idProvincia { get; set; }
        public int? idCanton { get; set; }
        public int? idSector { get; set; }
        public string? description { get; set; }

        [Range(0, 100000, ErrorMessage = "The {0} should be between {1} and {2} dollars")]
        public decimal? reward { get; set; }

        [DataType(DataType.DateTime)]
        [DateRange(ErrorMessage = "Value for {0} must be between {1} and {2}")]
        public DateTime? lastTimeSeen { get; set; }

        public string? linkMapSeen { get; set; }

        [MaxLength(6, ErrorMessage = "{0} must have a max of 6 urls")]
        public List<updatePostImgDTO>? urlImgs { get; set; }

    }
}