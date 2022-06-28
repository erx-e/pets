import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { environment } from "src/environments/environment";
import { CreateUserDTO, UpdateUserDTO } from "../models/user.model";

@Injectable({
  providedIn: "root",
})
export class UserService {
  constructor(private http: HttpClient) {}

  apiUrl = `${environment.API_URL}`;

  getProfile(id: number | null) {
    if (id != null) {
      return this.http.get(`${this.apiUrl}/user/get/${id}`);
    }
    return this.http.get(`${this.apiUrl}/user/get`);
  }

  update(dto: UpdateUserDTO){
    return this.http.put(`${this.apiUrl}/user/update`, dto)
  }

  create(dto: CreateUserDTO){
    return this.http.post(`${this.apiUrl}/user/create`, dto);
  }
}
