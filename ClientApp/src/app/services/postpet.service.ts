import { HttpClient, HttpParams } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { environment } from "src/environments/environment";
import { Response } from "../models/response.model";
import * as S3 from "aws-sdk/clients/s3";
import {
  CreatePostpetDTO,
  postpetView,
  UpdatePostpetDTO,
  img
} from "../models/postpet.model";
import { checkToken } from "../interceptors/token.interceptor";
import { checkLoading } from "../interceptors/loading.interceptor";

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
    return this.http.get<postpetView[]>(`${this.apiUrl}/postpet/get`, {
      params, context: checkLoading()
    });
  }

  getById(id: number) {
    return this.http.get<postpetView>(`${this.apiUrl}/postpet/get/${id}`);
  }

  getByIdUpdate(id: number) {
    return this.http.get<UpdatePostpetDTO>(`${this.apiUrl}/postpet/getUpdate/${id}`);
  }

  getByState(id: string, limit?: number, offset?: number) {
    let params = new HttpParams();
    params = params.set("limit", limit);
    params = params.set("offset", offset);
    return this.http.get<postpetView[]>(
      `${this.apiUrl}/postpet/getByState/${id}`,
      { params, context: checkLoading() }
    );
  }

  GetByFilter(stateId: string, petSpecieId?: number | null, petBreedId?: number | null , provinciaId?: number | null , cantonId?: number | null, sectorId?: number | null, date?: string | null, order?: number | null, limit?: number, offset?: number){
    let params = new HttpParams();
    if(limit != undefined || offset != undefined){
      params = params.set("limit", limit);
      params = params.set("offset", offset);
    }

    // params = params.set("stateId", stateId)
    params = (petSpecieId != null || petSpecieId != undefined) ? params.set("petSpecieId", petSpecieId) : params
    params = (petBreedId != null || petBreedId != undefined) ? params.set("petBreedId", petBreedId) : params
    params = (provinciaId != null || provinciaId != undefined) ? params.set("provinciaId", provinciaId) : params
    params = (cantonId != null || cantonId != undefined) ? params.set("cantonId", cantonId) : params
    params = (sectorId != null || sectorId != undefined) ? params.set("sectorId", sectorId) : params
    params = (date != null || date != undefined) ? params.set("date", date) : params
    params = (order != null || order != undefined) ? params.set("order", order) : params

    return this.http.get<postpetView[] | null>(
      `${this.apiUrl}/postpet/getByFilter/${stateId}`,
      { params, context: checkLoading() }
    );
  }

  update(dto: UpdatePostpetDTO) {
    return this.http.put<Response>(`${this.apiUrl}/postpet/update`, dto, {
      context: checkToken(),
    });
  }

  create(dto: CreatePostpetDTO) {
    return this.http.post<Response>(`${this.apiUrl}/postpet/create`, dto, {
      context: checkToken(),
    });
  }

  delete(id: number) {
    return this.http.delete<Response>(`${this.apiUrl}/postpet/delete/${id}`, {
      context: checkToken(),
    });
  }

  storage = new S3({
    region: this.region,
    accessKeyId: this.key_id,
    secretAccessKey: this.key_secret,
  });

  uploadImage(file: File, key: string) {
    return this.storage.putObject(
      { Key: key, Body: file, Bucket: "petslighthouse" },
      function (err, data) {
        if (err) {
          console.log("There was an error uploading your file: ", err);
          return false;
        }
        console.log("Successfully uploaded file.", data);
        return true;
      }
    );
  }

  deleteImg(key: string) {
    return this.storage.deleteObject({ Key: key, Bucket: "petslighthouse" }).promise();
  }
}
