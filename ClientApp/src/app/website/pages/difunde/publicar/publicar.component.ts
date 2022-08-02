import { Component, OnDestroy, OnInit } from "@angular/core";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { CreatePostpetDTO } from "src/app/models/postpet.model";
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

@Component({
  selector: "app-publicar",
  templateUrl: "./publicar.component.html",
  styleUrls: ["./publicar.component.scss"],
})
export class PublicarComponent implements OnInit, OnDestroy {
  constructor(
    private formBuilder: FormBuilder,
    private categoryService: CategoryService,
    private postpetService: PostpetService,
    private location: Location,
    private authService: AuthService
  ) {
    this.buildForm();
  }

  async ngOnDestroy(): Promise<void> {
    if (!this.publishedPost && this.imgUrls.length > 0) {
      for (let imgIndex in this.imgUrls) {
        console.log("eliminar");
        await this.deleteImg(parseInt(imgIndex), false);
      }
    }
  }

  urlBucket = `${environment.BUCKET_URL}`;

  // @ViewChildren("barList") barList: QueryList<ElementRef>;

  // ngAfterViewInit(){
  //   this.barList.changes.subscribe(()=> {
  //     var elemtn = this.barList.find((item, index)=> index === 0 )
  //     console.log(elemtn)
  //   })
  // }

  form: FormGroup;
  createdPost: CreatePostpetDTO;
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

  editedFileList: FileList;
  isEditedFileList: boolean = false;
  isUploadedFiles: boolean = false;

  disableSubmit: boolean = false;
  maxSixFiles: boolean = false;

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
  }

  private buildForm() {
    this.form = this.formBuilder.group({
      petName: [
        "",
        [
          Validators.required,
          Validators.minLength(2),
          Validators.maxLength(12),
          Validators.pattern(/^[Aa-zA-ZáéíóúÁÉÍÓÚÑñ ]*$/),
        ],
      ],
      idPetSpecie: ["", Validators.required],
      idPetBreed: [{ value: "", disabled: true }, Validators.required],
      reward: ["", [Validators.min(0), Validators.max(100000)]],
      idProvincia: ["", Validators.required],
      idCanton: [{ value: "", disabled: true }, Validators.required],
      idSector: [""],
      description: ["", Validators.required],
      lastTimeSeen: ["", [Validators.required, MyValidators.correctDate]],
      urlImgs: ["", Validators.required],
    });
  }

  crearPost() {
    if (this.form.valid) {
      this.createdPost = this.form.value;

      this.createdPost.urlImgs = this.imgUrls.map((imgs) => imgs[0]);
      this.createdPost.idUser = this.user.idUser;

      this.createdPost.idState = "B";
      // YYYY-MM-DD HH:mm:SS:ml
      console.log(this.createdPost);
      this.postpetService.create(this.createdPost).subscribe(() => {
        this.publishedPost = true;
        this.location.back();
      });
    }
    this.form.markAllAsTouched();
  }

  OnImgLoaded(id: number) {
    // let deleteIconDiv = element.parentElement.nextElementSibling as HTMLElement
    // deleteIconDiv.style.display = "block";
    // console.log(event)
    this.disableSubmit = true;
    console.log(this.disableSubmit);
    let uploadedImgs = document.getElementById(id.toString());
    uploadedImgs.style.display = "block";

    let deleteIconDiv = uploadedImgs.lastElementChild as HTMLElement;
    deleteIconDiv.style.display = "block";

    let progressContainerDiv = document.getElementById(
      (id + 6).toString()
    ) as HTMLElement;
    progressContainerDiv.style.display = "none";
    this.disableSubmit = false;
  }

  async uploadFile(event: Event) {
    console.log(event);
    if (this.isUploadedFiles) {
      await this.ngOnDestroy();
      this.imgNames = [];
      this.imgUrls = [];
    }
    console.log(this.imgUrls);
    this.state = "loading";
    const inputFile = event.target as HTMLInputElement;
    console.log(inputFile);
    console.log(inputFile.files);
    let imgs: FileList = inputFile.files;
    console.log(imgs);
    if (imgs.length > 0 && imgs.length < 7) {
      this.maxSixFiles = false;
      for (let i = 0; i < imgs.length; i++) {
        let img: File = imgs.item(i);
        if (img) {
          let uploadedImgs = document.getElementById(i.toString());
          console.log(uploadedImgs);
          uploadedImgs.style.display = "block";
          console.log(uploadedImgs.style.display);
          let progressBarDiv = document.getElementById((i + 6).toString());
          console.log(progressBarDiv);
          progressBarDiv.style.display = "block";
          let barDiv = progressBarDiv.firstElementChild as HTMLElement;
          this.imgNames.push(img.name);
          console.log(barDiv);
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
              console.log(progressPercentage, i);
              barDiv.style.width = progressPercentage + "%";
              console.log(barDiv.style.width);
              if (progressPercentage == 100) {
                if (this.imgUrls.length > 0) {
                  for (let i = 0; i < this.imgUrls.length; i++) {
                    let item = this.imgUrls[i];
                    if (item == null) {
                      setTimeout(() => {
                        this.imgUrls[i] = [imgUrl, img.name];
                      }, 1000);
                      notNull = false;
                      break;
                    }
                  }
                }
                if (notNull) {
                  setTimeout(() => this.imgUrls.push([imgUrl, img.name]), 1000);
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
  }

  async deleteImg(index: number, removeFiles: boolean) {
    if (this.imgUrls[index]) {
      var keyImg = this.imgUrls[index][0].split("/").pop();
      var imgName = this.imgUrls[index][1];
      console.log(keyImg);

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
      if (imgName !== file.name) dt.items.add(file);
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

  get petNameField() {
    return this.form.get("petName");
  }

  get petNameFieldValid() {
    return this.petNameField.touched && this.petNameField.valid;
  }

  get petNameFieldInvalid() {
    return this.petNameField.touched && this.petNameField.invalid;
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

  get descriptionField() {
    return this.form.get("description");
  }

  get descriptionFieldValid() {
    return this.descriptionField.touched && this.descriptionField.valid;
  }

  get descriptionFieldInvalid() {
    return this.descriptionField.touched && this.descriptionField.invalid;
  }

  get rewardField() {
    return this.form.get("reward");
  }

  get rewardFieldValid() {
    return this.rewardField.touched && this.rewardField.valid;
  }

  get rewardFieldInvalid() {
    return this.rewardField.touched && this.rewardField.invalid;
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
