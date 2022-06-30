import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { environment } from "src/environments/environment";
import { authUser, UserView, authUserResponse } from "../models/user.model";

@Injectable({
  providedIn: "root",
})
export class AuthService {
  constructor(private http: HttpClient) {}

  private apiUrl = `${environment.API_URL}/user`;

  login(auth: authUser): Observable<authUserResponse>{
    return this.http.post<authUserResponse>(`${this.apiUrl}/login`, auth);
  }

  getProfile(id: number | null): Observable<UserView> {
    if (id != null) {
      return this.http.get<UserView>(`${this.apiUrl}/get/${id}`);
    }
    return this.http.get<UserView>(`${this.apiUrl}/get`);
  }
}
