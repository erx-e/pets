export interface UserView {
  name: string;
  email: string;
  cellNumber: string;
  facebookProfile: string;
}

export interface CreateUserDTO extends UserView {
  password: string;
}

export interface UpdateUserDTO extends Partial<CreateUserDTO> {
  idUser: number;
}
