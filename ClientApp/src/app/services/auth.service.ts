import { HttpClient, HttpErrorResponse, HttpStatusCode } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { BehaviorSubject, Observable, throwError } from "rxjs";
import { tap, switchMap, catchError } from "rxjs/operators";
import { environment } from "src/environments/environment";
import { authUser, UserView, authUserResponse } from "../models/user.model";
import { TokenService } from "./token.service";
import { checkToken } from "../interceptors/token.interceptor"

@Injectable({
  providedIn: "root",
})
export class AuthService {
  constructor(private http: HttpClient, private tokenService: TokenService) {}


  private apiUrl = `${environment.API_URL}/user`;

  login(auth: authUser): Observable<authUserResponse> {
    return this.http
      .post<authUserResponse>(`${this.apiUrl}/login`, auth, {context: checkToken()})
      .pipe(tap((response) => this.tokenService.saveToken(response.token)));
  }

  getProfile(id?: number) {
    if (id != undefined) {
      return this.http.get<UserView>(`${this.apiUrl}/get/${id}`, {context: checkToken()});
    }

    // const headers = new HttpHeaders();
    // headers.set("Authorization", `Bearer ${token}`);

    return this.http.get<UserView>(`${this.apiUrl}/get`, {context: checkToken()})
    .pipe(
      catchError((error: HttpErrorResponse)=> {
        if(error.status === HttpStatusCode.Unauthorized){
          return throwError("No estas autorizado")
        }
        return throwError("Bad request");
      })
    );
      // .subscribe((profile) => this.user.next(profile));
    //headers : {
    //   Authorization: `Bearer ${token}`,
    // },
  }

  loginAndGetProfile(auth: authUser) {
    this.login(auth).pipe(switchMap(() => this.getProfile()));
  }
}
