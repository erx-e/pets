import { HttpClient, HttpParams } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { environment } from "src/environments/environment";
import { Response } from "../models/response.model";
import * as S3 from "aws-sdk/clients/s3";
import { CreatePostpetDTO, postpetView, UpdatePostpetDTO } from "../models/postpet.model";
import { checkToken } from "../interceptors/token.interceptor";

@Injectable({
  providedIn: "root",
})
export class PostpetService {
  constructor(private http: HttpClient) {}

  private apiUrl = `${environment.API_URL}`;
  private key_id = environment.AWS_KEY_ID;
  private key_secret = environment.AWS_KEY_SECRET;
  private region = environment.AWS_REGION;



  getAll(limit?: number, offset?: number) {
    let params = new HttpParams();
    params = params.set("limit", limit);
    params = params.set("offset", offset);
    return this.http.get<postpetView[]>(`${this.apiUrl}/postpet/get`, { params });
  }

  getById(id: number) {
    return this.http.get<Response>(`${this.apiUrl}/postpet/get/${id}`);
  }

  update(dto: UpdatePostpetDTO) {
    return this.http.put<Response>(`${this.apiUrl}/postpet/update`, dto, {context: checkToken()});
  }

  create(dto: CreatePostpetDTO) {
    return this.http.post<Response>(`${this.apiUrl}/postpet/create`, dto, {context: checkToken()});
  }

  delete(id: number){
    return this.http.delete<Response>(`${this.apiUrl}/postpet/delete/${id}`, {context: checkToken()});
  }

  storage = new S3({
    region: this.region,
    accessKeyId: this.key_id,
    secretAccessKey: this.key_secret,
  });

  uploadImage(any: any) {
    this.storage.putObject(any);
  }
}
