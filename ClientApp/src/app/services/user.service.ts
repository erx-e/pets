import {
  HttpClient,
  HttpErrorResponse,
  HttpStatusCode,
} from "@angular/common/http";
import { Injectable } from "@angular/core";
import { environment } from "src/environments/environment";
import { CreateUserDTO, UpdateUserDTO } from "../models/user.model";
import { Response } from "../models/response.model";
import { catchError, tap } from "rxjs/operators";
import { TokenService } from "./token.service";
import { AuthService } from "./auth.service"
import { throwError } from "rxjs";

@Injectable({
  providedIn: "root",
})
export class UserService {
  constructor(private http: HttpClient, private tokenService: TokenService, private authService: AuthService) {}

  private apiUrl = `${environment.API_URL}/user`;

  update(dto: UpdateUserDTO) {
    return this.http.put(`${this.apiUrl}/update`, dto);
  }

  create(dto: CreateUserDTO) {
    return this.http.post<Response>(`${this.apiUrl}/create`, dto).pipe(
      catchError((error: HttpErrorResponse) => {
        if (error.status === HttpStatusCode.BadRequest) {
          return throwError("Email already registered");
        }
      }),
      tap((response: Response) => {
        if (response.token) {
          this.tokenService.saveToken(response.token);
          this.authService.getCurrentUser();
          console.log(response.token)
        }
      })
    );
  }

  delete(id: number) {
    return this.http.delete(`${this.apiUrl}/delete/${id}`);
  }
}
