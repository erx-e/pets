import { Component, OnDestroy, OnInit } from "@angular/core";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import {
  CreatePostpetDTO,
  UpdatePostpetDTO,
  updateImg,
} from "src/app/models/postpet.model";
import { Location } from "@angular/common";
import { CategoryService } from "src/app/services/category.service";
import { v4 as uuidv4 } from "uuid";
import {
  breed,
  canton,
  provincia,
  sector,
  specie,
} from "../../../../models/category.model";
import { switchMap } from "rxjs/operators";
import { PostpetService } from "src/app/services/postpet.service";
import { environment } from "src/environments/environment";
import { AuthService } from "src/app/services/auth.service";
import { UserView } from "src/app/models/user.model";
import { MyValidators } from "src/app/validators/validators";
import { Observable } from "rxjs";
import { ActivatedRoute } from "@angular/router";

@Component({
  selector: "app-publicar",
  templateUrl: "./publicar.component.html",
  styleUrls: ["./publicar.component.scss"],
})
export class PublicarComponent implements OnInit, OnDestroy {
  constructor(
    private formBuilder: FormBuilder,
    private router: ActivatedRoute,
    private categoryService: CategoryService,
    private postpetService: PostpetService,
    private location: Location,
    private authService: AuthService
  ) {
    this.buildForm();
  }

  postpetId: string | null;
  postpet!: UpdatePostpetDTO | null;

  urlBucket = `${environment.BUCKET_URL}`;

  // @ViewChildren("barList") barList: QueryList<ElementRef>;

  // ngAfterViewInit(){
  //   this.barList.changes.subscribe(()=> {
  //     var elemtn = this.barList.find((item, index)=> index === 0 )
  //     console.log(elemtn)
  //   })
  // }

  form: FormGroup;
  createdPost!: CreatePostpetDTO;
  updatePost: UpdatePostpetDTO = {
    idPostPet: null,
    idUser: null,
    petName: null,
    idCanton: null,
    idPetBreed: null,
    idPetSpecie: null,
    idProvincia: null,
    idSector: null,
    idState: null,
    description: null,
    reward: null,
    lastTimeSeen: null,
    urlImgs: null,
  };
  publishedPost: boolean = false;
  species: specie[] = [];
  specieId: number;
  breeds: breed[] = [];
  provincias: provincia[] = [];
  provinciaId: number;
  cantones: canton[] = [];
  cantonId: number;
  sectores: sector[] = [];
  dateInvalid: boolean = false;
  user: UserView;

  imgsUploading: number[] = [0, 1, 2, 3, 4, 5];
  imgsProgressBar: number[] = [6, 7, 8, 9, 10, 11];
  imgNames: string[] = [];
  imgUrls: [string, string][] = [];
  private _imgUrlsOriginal: [string, string][] = [];

  get imgUrlsOriginal(): [string, string][] {
    return this._imgUrlsOriginal;
  }
  set imgUrlsOriginal(value: [string, string][]) {
    if (value !== this._imgUrlsOriginal) {
      this._imgUrlsOriginal = value;
      console.log(JSON.parse(JSON.stringify(this.imgUrlsOriginal)));
    }
  }

  editedFileList: FileList;
  isEditedFileList: boolean = false;
  isUploadedFiles: boolean = false;

  disableSubmit: boolean = false;
  maxSixFiles: boolean = false;

  editPost: boolean = false;
  publishingPost: boolean = false;

  state: "loading" | "succes" | "initial" | "error";

  ngOnInit(): void {
    this.authService.user$.subscribe((data) => (this.user = data));
    this.toggleDisabledBreed();
    this.toggleDisabledCanton();
    this.getSpecies();
    this.getBreedsBySpecie();
    this.getProvincias();
    this.getCantonesByProv();
    this.getSectoresByCanton();
    window.onbeforeunload = () => this.ngOnDestroy();

    this.router.paramMap
      .pipe(
        switchMap((params) => {
          this.postpetId = params.get("id");
          if (this.postpetId) {
            return this.postpetService.getByIdUpdate(parseInt(this.postpetId));
          }
          return null;
        })
      )
      .subscribe((data) => {
        this.postpet = data;
        if (this.postpet) {
          // this.form.patchValue(this.postpet)
          this.petSpecieField.setValue(this.postpet.idPetSpecie);
          this.petBreedField.setValue(this.postpet.idPetBreed);
          this.provinciaField.setValue(this.postpet.idProvincia);
          this.cantonField.setValue(this.postpet.idCanton);
          if (this.postpet.idSector) {
            this.sectorField.setValue(this.postpet.idSector);
          }
          this.descriptionField.setValue(this.postpet.description);
          this.lastTimeSeenField.setValue(this.postpet.lastTimeSeen);

          this.editPost = true;
          let petSpecie1 = document.getElementById(
            "especie1"
          ) as HTMLInputElement;
          let petSpecie2 = document.getElementById(
            "especie2"
          ) as HTMLInputElement;
          if (petSpecie1.value) {
            petSpecie1.click();
          } else {
            petSpecie2.click();
          }
          this.form.markAllAsTouched();
          // this.petSpecieField.setValue(this.postpet.)
          this.imgUrls = this.postpet.urlImgs.map((img) => [
            img.url,
            img.idImage.toString(),
          ]);
          console.log(this.imgUrls);
          this.imgUrlsOriginal = this.postpet.urlImgs.map((img) => [
            img.url,
            img.idImage.toString(),
          ]);
          console.log(JSON.parse(JSON.stringify(this.imgUrlsOriginal)));
        }
      });
  }

  async ngOnDestroy(): Promise<void> {
    console.log(
      JSON.parse(JSON.stringify(this.imgUrlsOriginal)),
      "original destroy"
    );
    this.imgUrlsOriginal = this.postpet.urlImgs.map((img) => {
      return [img.url, img.idImage.toString()];
    });

    if (!this.publishedPost && this.imgUrls.length > 0 && !this.editPost) {
      for (let imgIndex in this.imgUrls) {
        console.log("eliminar");
        if (this.imgUrls[imgIndex][1]) {
          await this.deleteImg(parseInt(imgIndex), false);
        }
      }
    } else if (this.editPost && !this.publishedPost) {
      for (let imgIndex in this.imgUrls) {
        console.log(this.imgUrls[imgIndex]);
        console.log(this.imgUrlsOriginal[imgIndex] == null);
        if(this.imgUrlsOriginal[imgIndex]){          
          console.log(
            this.imgUrlsOriginal[imgIndex][0] != this.imgUrls[imgIndex][0]
          );
        }
        if (
          this.imgUrls[imgIndex] &&
          (this.imgUrlsOriginal[imgIndex] == null ||
            this.imgUrlsOriginal[imgIndex][0] != this.imgUrls[imgIndex][0])
        ) {
          console.log("eliminar");
          await this.deleteImg(parseInt(imgIndex), false);
          console.log("eliminar");
        }
      }
    }
    console.log(
      JSON.parse(JSON.stringify(this.imgUrlsOriginal)),
      "original destroy"
    );
  }

  private buildForm() {
    this.form = this.formBuilder.group({
      idPetSpecie: ["", Validators.required],
      idPetBreed: [{ value: "", disabled: true }, Validators.required],
      idProvincia: ["", Validators.required],
      idCanton: [{ value: "", disabled: true }, Validators.required],
      idSector: [""],
      description: ["", Validators.required],
      lastTimeSeen: ["", [Validators.required, MyValidators.correctDate]],
      urlImgs: ["", Validators.required],
    });
  }

  crearPost() {
    if (this.editPost) {
      this.urlImgsField.clearValidators();
      this.urlImgsField.updateValueAndValidity();
    }
    if (this.form.valid && this.imgUrls.length > 0) {
      if (this.editPost) {
        this.updatePost.idPostPet = this.postpet.idPostPet;

        this.updatePost.idPetSpecie =
          this.postpet.idPetSpecie != this.petSpecieField.value
            ? this.petSpecieField.value
            : null;
        this.updatePost.idPetBreed =
          this.postpet.idPetBreed != this.petBreedField.value
            ? this.petBreedField.value
            : null;
        this.updatePost.idProvincia =
          this.postpet.idProvincia != this.provinciaField.value
            ? this.provinciaField.value
            : null;
        this.updatePost.idCanton =
          this.postpet.idCanton != this.cantonField.value
            ? this.cantonField.value
            : null;
        this.updatePost.idSector =
          this.postpet.idSector != this.sectorField.value
            ? this.sectorField.value
            : null;
        this.updatePost.description =
          this.postpet.description != this.descriptionField.value
            ? this.descriptionField.value
            : null;
        this.updatePost.lastTimeSeen =
          this.postpet.lastTimeSeen != this.lastTimeSeenField.value
            ? this.lastTimeSeenField.value
            : this.lastTimeSeenField.value;

        this.updatePost.urlImgs =
          this.imgUrlsOriginal != this.imgUrls
            ? this.imgUrls.map<updateImg>((imgs, index) => {
                if (imgs) {
                  return {
                    url: imgs[0],
                    idImage: /^[0-9]+$/.test(imgs[1])
                      ? parseInt(imgs[1])
                      : null,
                  };
                } else if (this.imgUrlsOriginal[index]) {
                  return {
                    url: null,
                    idImage: parseInt(this.imgUrlsOriginal[index][1]),
                  };
                }
              })
            : null;

        this.updatePost.idUser = this.user.idUser;
        this.updatePost.idState = "E";
        // YYYY-MM-DD HH:mm:SS:ml
        this.publishedPost = true;
        this.postpetService.update(this.updatePost).subscribe(async () => {
          console.log(this.publishedPost);
          this.imgUrlsOriginal = this.postpet.urlImgs.map((img) => {
            return [img.url, img.idImage.toString()];
          });
          for (let imgIndex in this.imgUrls) {
            console.log("eliminar");
            if (
              this.imgUrlsOriginal[imgIndex] &&
              ((!this.imgUrls[imgIndex] && this.imgUrlsOriginal[imgIndex]) ||
                this.imgUrlsOriginal[imgIndex][0] != this.imgUrls[imgIndex][0])
            ) {
              console.log(this.imgUrlsOriginal[imgIndex]);
              await this.deleteImg(parseInt(imgIndex), false);
            }
          }
          this.location.back();
        });
      } else {
        this.createdPost = this.form.value;

        this.createdPost.urlImgs = this.imgUrls.map((imgs) => imgs[0]);
        this.createdPost.idUser = this.user.idUser;

        this.createdPost.idState = "E";
        // YYYY-MM-DD HH:mm:SS:ml
        this.postpetService.create(this.createdPost).subscribe(() => {
          this.publishedPost = true;
          this.location.back();
        });
      }
    }
    this.form.markAllAsTouched();
  }

  OnImgLoaded(id: number) {
    // let deleteIconDiv = element.parentElement.nextElementSibling as HTMLElement
    // deleteIconDiv.style.display = "block";
    // console.log(event)

    this.disableSubmit = true;
    let uploadedImgs = document.getElementById(id.toString());
    uploadedImgs.style.display = "block";

    let imgContainer = uploadedImgs.firstElementChild as HTMLElement;
    imgContainer.style.display = "block";

    let deleteIconDiv = imgContainer.lastElementChild as HTMLElement;
    deleteIconDiv.style.display = "block";

    let progressContainerDiv = document.getElementById(
      (id + 6).toString()
    ) as HTMLElement;
    progressContainerDiv.style.display = "none";
    this.disableSubmit = false;
  }

  async uploadFile(event: Event) {
    console.log(
      JSON.parse(JSON.stringify(this.imgUrlsOriginal)),
      "original upload"
    );

    if (this.isUploadedFiles) {
      if (this.editPost) {
        await this.ngOnDestroy();
        this.imgUrls = this.imgUrlsOriginal;
      } else {
        await this.ngOnDestroy();
        this.imgNames = [];
        this.imgUrls = [];
      }
    }
    if (this.imgUrls.length >= 6) {
      this.maxSixFiles = true;
      return;
    }
    console.log(this.imgUrls);
    this.state = "loading";
    const inputFile = event.target as HTMLInputElement;

    let imgs: FileList = inputFile.files;

    if (imgs.length > 0 && imgs.length < 7) {
      this.maxSixFiles = false;
      for (let i = 0; i < imgs.length; i++) {
        let img: File = imgs.item(i);
        if (img) {
          let uploadedImgs = document.getElementById(i.toString());
          uploadedImgs.style.display = "block";
          let progressBarDiv = document.getElementById((i + 6).toString());
          progressBarDiv.style.display = "block";
          let barDiv = progressBarDiv.firstElementChild as HTMLElement;
          this.imgNames.push(img.name);
          let rndName = uuidv4();
          let extension = img.name.split(".").pop();
          let key = `${rndName}.${extension}`;
          let imgUrl = `${this.urlBucket}/${key}`;
          let notNull = true;
          this.postpetService
            .uploadImage(img, key)
            .on("httpUploadProgress", (progress) => {
              console.log("XD", i);
              let progressPercentage = Math.round(
                (progress.loaded / progress.total) * 100
              );
              barDiv.style.width = progressPercentage + "%";
              if (progressPercentage == 100) {
                setTimeout(() => {
                  progressBarDiv.style.display = "none";
                }, 2000);
                if (this.imgUrls.length > 0) {
                  for (let i = 0; i < this.imgUrls.length; i++) {
                    let item = this.imgUrls[i];
                    if (item == null) {
                      setTimeout(() => {
                        this.putImgByIndex(i, imgUrl, img.name);
                        console.log(
                          JSON.parse(JSON.stringify(this.imgUrlsOriginal)),
                          "setTimeOut"
                        );
                      }, 1000);
                      notNull = false;
                      break;
                    }
                  }
                }
                if (notNull) {
                  setTimeout(
                    () => this.imgUrls.push([imgUrl, `i${img.name}`]),
                    1000
                  );
                }
              }
              this.isUploadedFiles = true;
            });
        }
      }
    } else {
      this.maxSixFiles = true;
      inputFile.value = "";
    }
    if (this.state == "loading") {
      this.state = "succes";
    } else {
      this.state = "error";
    }
    console.log(
      JSON.parse(JSON.stringify(this.imgUrlsOriginal)),
      "original upload"
    );
  }

  putImgByIndex(index: number, imgUrl: string, imgName: string) {
    this.imgUrls[index] = [imgUrl, `i${imgName}`];
    this.imgUrlsOriginal = this.postpet.urlImgs.map((img) => {
      return [img.url, img.idImage.toString()];
    });
  }

  async deleteImg(index: number, removeFiles: boolean) {
    this.imgUrlsOriginal = this.postpet.urlImgs.map((img) => {
      return [img.url, img.idImage.toString()];
    });

    console.log(
      JSON.parse(JSON.stringify(this.imgUrlsOriginal)),
      "original delete"
    );

    if (this.imgUrls[index]) {
      var keyImg = this.imgUrls[index][0].split("/").pop();
      var imgName = this.imgUrls[index][1];
      console.log(keyImg);

      if (this.editPost) {
        console.log(this.editPost);
        console.log(this.publishedPost);
        if (this.publishedPost) {
          keyImg = this.imgUrlsOriginal[index][0].split("/").pop();
          console.log(keyImg);
          await this.postpetService
            .deleteImg(keyImg)
            .then((data) => {
              console.log(data);
            })
            .catch((err) => console.log(err));
        }
        console.log(this.imgUrlsOriginal);
        if (
          !this.imgUrlsOriginal[index] ||
          this.imgUrls[index][0] != this.imgUrlsOriginal[index][0]
        ) {
          await this.postpetService
            .deleteImg(keyImg)
            .then(() => {
              if (removeFiles) {
                this.removeFileFromFileList(imgName);
              }
              this.imgUrls[index] = null;
              this.imgNames[index] = null;
              console.log("sex");
            })
            .catch((err) => console.log(err));
          return;
        } else {
          this.imgUrls[index] = null;
          return;
        }
      } else {
        await this.postpetService
          .deleteImg(keyImg)
          .then(() => {
            if (removeFiles) {
              this.removeFileFromFileList(imgName);
            }
            this.imgUrls[index] = null;
            this.imgNames[index] = null;
            var progressContainer = document.getElementById(index.toString());
            progressContainer.style.display = "none";
            var barDiv = progressContainer.lastElementChild
              .firstElementChild as HTMLElement;
            barDiv.style.width = "1%";
            console.log("sex");
          })
          .catch((err) => console.log(err));
      }
    }
    console.log(
      JSON.parse(JSON.stringify(this.imgUrlsOriginal)),
      "original delete"
    );
  }

  removeFileFromFileList(imgName: string) {
    let dt = new DataTransfer();
    let inputFile = document.getElementById("formImgs") as HTMLInputElement;
    let { files } = inputFile;
    if (files.length == 1) {
      inputFile.value = "";
      this.urlImgsField.setValue("");
    }

    for (let i = 0; i < files.length; i++) {
      let file = files[i];
      if (imgName !== `i${file.name}`) dt.items.add(file);
    }

    inputFile.files = dt.files;
    this.isEditedFileList = true;
    this.editedFileList = dt.files;
  }

  addFileFromFileList(newFileList: FileList, oldFileList: FileList) {
    let dt = new DataTransfer();

    for (let i = 0; i < oldFileList.length; i++) {
      let fl = oldFileList[i];
      dt.items.add(fl);
    }

    for (let i = 0; i < newFileList.length; i++) {
      let fl = newFileList[i];
      dt.items.add(fl);
    }

    return dt.files;
  }

  toggleDisabledBreed() {
    this.petSpecieField.valueChanges.subscribe(() => {
      if (this.petSpecieField.hasError("required")) {
        this.disableBreedField();
      } else {
        this.enableBreedField();
      }
    });
  }

  toggleDisabledCanton() {
    this.provinciaField.valueChanges.subscribe(() => {
      if (this.provinciaField.value) {
        this.cantonField.enable();
      } else {
        this.cantonField.setValue("");
        this.cantonField.disable();
      }
    });
  }

  enableBreedField() {
    this.petBreedField.enable();
  }

  disableBreedField() {
    this.petBreedField.disable();
  }

  private getSpecies() {
    this.categoryService.getSpecies().subscribe((species: specie[]) => {
      this.species = species;
    });
  }

  private getBreedsBySpecie() {
    this.petSpecieField.valueChanges
      .pipe(
        switchMap((id) => {
          return this.categoryService.getBreedsBySpecie(id);
        })
      )
      .subscribe((breeds: breed[]) => (this.breeds = breeds));
  }

  private getProvincias() {
    this.categoryService
      .getProvincias()
      .subscribe((provincias: provincia[]) => {
        this.provincias = provincias;
      });
  }

  private getCantonesByProv() {
    this.provinciaField.valueChanges
      .pipe(
        switchMap((id) => {
          console.log(id);
          return this.categoryService.getCantonesByProv(id);
        })
      )
      .subscribe((cantones: canton[]) => (this.cantones = cantones));
  }

  private getSectoresByCanton() {
    this.cantonField.valueChanges
      .pipe(
        switchMap((id) => {
          if (id) {
            console.log(id);
            return this.categoryService.getSectoresByCanton(id);
          } else {
            return new Observable<null>();
          }
        })
      )
      .subscribe((sectores: sector[] | null) => {
        console.log(sectores);
        this.sectores = sectores;
      });
  }

  get petSpecieField() {
    return this.form.get("idPetSpecie");
  }

  get petSpecieFieldValid() {
    return this.petSpecieField.touched && this.petSpecieField.valid;
  }

  get petSpecieFieldInvalid() {
    return this.petSpecieField.touched && this.petSpecieField.invalid;
  }

  get petBreedField() {
    return this.form.get("idPetBreed");
  }

  get petBreedFieldValid() {
    return this.petBreedField.touched && this.petBreedField.valid;
  }

  get petBreedFieldInvalid() {
    return this.petBreedField.touched && this.petBreedField.invalid;
  }

  get provinciaField() {
    return this.form.get("idProvincia");
  }

  get provinciaFieldValid() {
    return this.provinciaField.touched && this.provinciaField.valid;
  }

  get provinciaFieldInvalid() {
    return this.provinciaField.touched && this.provinciaField.invalid;
  }

  get cantonField() {
    return this.form.get("idCanton");
  }

  get cantonFieldValid() {
    return this.cantonField.touched && this.cantonField.valid;
  }

  get cantonFieldInvalid() {
    return this.cantonField.touched && this.cantonField.invalid;
  }

  get sectorField() {
    return this.form.get("idSector");
  }

  get descriptionField() {
    return this.form.get("description");
  }

  get descriptionFieldValid() {
    return this.descriptionField.touched && this.descriptionField.valid;
  }

  get descriptionFieldInvalid() {
    return this.descriptionField.touched && this.descriptionField.invalid;
  }

  get lastTimeSeenField() {
    return this.form.get("lastTimeSeen");
  }

  get lastTimeSeenFieldValid() {
    return this.lastTimeSeenField.touched && this.lastTimeSeenField.valid;
  }

  get lastTimeSeenFieldInvalid() {
    return this.lastTimeSeenField.touched && this.lastTimeSeenField.invalid;
  }

  get urlImgsField() {
    return this.form.get("urlImgs");
  }

  get urlImgsFieldValid() {
    return (
      this.urlImgsField.touched &&
      this.urlImgsField.valid &&
      this.maxSixFiles == false
    );
  }

  get urlImgsFieldInvalid() {
    return (
      this.urlImgsField.touched && this.urlImgsField.invalid && this.maxSixFiles
    );
  }
}
