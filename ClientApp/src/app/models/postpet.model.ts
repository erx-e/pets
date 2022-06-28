export interface postpetView {
  id: string;
  userName: string;
  petName: string;
  petState: string;
  petSpecie: string;
  petBreed: string | null;
  provinciaName: string;
  cantonName: string;
  sectorName: string | null;
  description: string;
  reward: number | null;
  lastTimeSeen: Date;
  linkMapSeen: string | null;
  imgs: img[];
}

export interface CreatePostpetDTO {
  idUser: number;
  petName: string;
  idState: number;
  idPetSpecie: number;
  idPetBreed: number | null;
  idProvincia: number;
  idSector: number | null;
  description: string;
  reward: number | null;
  lastTimeSeen: Date;
  linkMapSeen: string;
  urlImgs: img[];
}

export interface UpdatePostpetDTO
  extends Omit<Partial<CreatePostpetDTO>, "idUser"> {
  idUser: number;
  idPostPet: number;
}

interface img {
  fileName: string;
  url: string;
}
