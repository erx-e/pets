import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { environment } from "src/environments/environment";
import { UserView, CreateUserDTO, UpdateUserDTO, authUser } from "../models/user.model";
import { Observable, of } from "rxjs";

@Injectable({
  providedIn: "root",
})
export class UserService {
  constructor(private http: HttpClient) {}

  private apiUrl = `${environment.API_URL}`;

  login(authRequest: authUser){
    return this.http.post(`${this.apiUrl}/user/login`, authRequest);
  }

  getProfile(id: number | null) : Observable<UserView> {
    if (id != null) {
      return this.http.get<UserView>(`${this.apiUrl}/user/get/${id}`);
    }
    return this.http.get<UserView>(`${this.apiUrl}/user/get`);
  }

  update(dto: UpdateUserDTO) {
    return this.http.put(`${this.apiUrl}/user/update`, dto);
  }

  create(dto: CreateUserDTO) {
    return this.http.post(`${this.apiUrl}/user/create`, dto);
  }

  delete(id: number){
    return this.http.delete(`${this.apiUrl}/user/delete/${id}`);
  }
}
