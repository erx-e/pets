import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { environment } from "src/environments/environment";
import { UserView, CreateUserDTO, UpdateUserDTO, authUser } from "../models/user.model";
import { Observable} from "rxjs";

@Injectable({
  providedIn: "root",
})
export class UserService {
  constructor(private http: HttpClient) {}

  private apiUrl = `${environment.API_URL}/user`;

  login(authRequest: authUser){
    return this.http.post(`${this.apiUrl}/login`, authRequest);
  }



  update(dto: UpdateUserDTO) {
    return this.http.put(`${this.apiUrl}/update`, dto);
  }

  create(dto: CreateUserDTO) {
    return this.http.post(`${this.apiUrl}/create`, dto);
  }

  delete(id: number){
    return this.http.delete(`${this.apiUrl}/delete/${id}`);
  }
}
