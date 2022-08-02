import { Component, OnInit } from "@angular/core";
import { FormBuilder, FormGroup } from "@angular/forms";
import { postpetView, PostpetFilter } from "src/app/models/postpet.model";
import {
  breed,
  canton,
  provincia,
  sector,
} from "../../../models/category.model";
import { switchMap } from "rxjs/operators";
import { CategoryService } from "src/app/services/category.service";
import { PostpetService } from "src/app/services/postpet.service";
import { Observable } from "rxjs";
import { MyValidators } from "src/app/validators/validators";
import { NoScrollService } from "../../../services/no-scroll.service";

@Component({
  selector: "app-encuentra",
  templateUrl: "./difunde.component.html",
  styleUrls: ["./difunde.component.scss"],
})
export class DifundeComponent implements OnInit {
  constructor(
    private postpetService: PostpetService,
    private formBuilder: FormBuilder,
    private categoryService: CategoryService,
    private noScrollService: NoScrollService
  ) {
    this.buildForm();
  }

  notMorePostpet: boolean = false;

  petSpecieId: number | null = null;
  petBreedId: number | null = null;
  provinciaId: number | null = null;
  cantonId: number | null = null;
  sectorId: number | null = null;
  date: string | null = null;

  breeds: breed[] = [];
  provincias: provincia[] = [];
  cantones: canton[] = [];
  sectores: sector[] = [];

  filterListPost: postpetView[] = [];

  form: FormGroup;
  postspet: postpetView[] = [];
  limit: number = 10;
  offset: number = 0;
  filter: boolean = false;

  ngOnInit(): void {
    this.postpetService.getByState("B", 10, 0).subscribe((data) => {
      this.offset += this.limit;
      this.postspet = data;
    });
    this.getBreedsBySpecie();
    this.getProvincias();
    this.getCantonesByProv();
    this.getSectoresByCanton();

    this.toggleDisabledBreed();
    this.toggleDisabledCanton();
    this.toggleDisabledSector();
  }

  overflowHidden() {
    if (this.filter) {
      this.noScrollService.noScrollEmit(true);
    } else {
      this.noScrollService.noScrollEmit(false);
    }
  }

  onLoadMore() {
    this.postpetService
      .GetByFilter(
        "B",
        this.petSpecieId,
        this.petBreedId,
        this.provinciaId,
        this.cantonId,
        this.sectorId,
        this.date,
        this.limit,
        this.offset
      )
      .subscribe((data) => {
        if (data != null) {
          this.postspet = this.postspet.concat(data);
          this.offset += this.limit;
          if (data.length < 10) {
            this.notMorePostpet = true;
          }
        } else {
          this.notMorePostpet = true;
        }
      });
  }

  private buildForm() {
    this.form = this.formBuilder.group({
      idPetSpecie: [""],
      idPetBreed: [{ value: "", disabled: "true" }],
      idProvincia: [""],
      idCanton: [{ value: "", disabled: "true" }],
      idSector: [{ value: "", disabled: "true" }],
      lastTimeSeen: ["", MyValidators.correctDate],
    });
  }

  applyFilter(event: SubmitEvent) {
    if (this.form.valid) {
      if (event.submitter.classList.contains("btn-apply")) {
        this.limit = 10;
        this.offset = 0;

        this.petSpecieId =
          this.petSpecieField.value != "" ? this.petSpecieField.value : null;

        this.petBreedId =
          this.petBreedField.value != "" ? this.petBreedField.value : null;

        this.provinciaId =
          this.provinciaField.value != "" ? this.provinciaField.value : null;

        this.cantonId =
          this.cantonField.value != "" ? this.cantonField.value : null;

        this.sectorId =
          this.sectorField.value != "" ? this.sectorField.value : null;

        this.date =
          this.lastTimeSeenField.value != ""
            ? this.lastTimeSeenField.value
            : null;

        this.postpetService
          .GetByFilter(
            "B",
            this.petSpecieId,
            this.petBreedId,
            this.provinciaId,
            this.cantonId,
            this.sectorId,
            this.date,
            this.limit,
            this.offset
          )
          .subscribe((data) => {
            this.postspet = data;
            this.notMorePostpet = false;
            this.filter = false;
            this.overflowHidden();
            this.offset += this.limit;
          });
      } else {
        this.petSpecieField.setValue("");
        this.petBreedField.setValue("");
        this.provinciaField.setValue("");
        this.cantonField.setValue("");
        this.sectorField.setValue("");
        this.lastTimeSeenField.setValue("");

        this.limit = 10;
        this.offset = 0;

        this.postpetService
          .getByState("B", this.limit, this.offset)
          .subscribe((data) => {
            this.postspet = this.postspet.concat(data);
            this.notMorePostpet = false;
            this.filter = false;
            this.overflowHidden();
            this.offset += this.limit;
          });
      }
    } else {
      this.form.markAllAsTouched();
    }
  }

  toggleFilter() {
    this.filter = !this.filter;
    this.overflowHidden();
  }

  toggleDisabledBreed() {
    this.petSpecieField.valueChanges.subscribe(() => {
      if (this.petSpecieField.value) {
        this.petBreedField.enable();
      } else {
        this.petBreedField.setValue("");
        this.petBreedField.disable();
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

  toggleDisabledSector() {
    this.cantonField.valueChanges.subscribe(() => {
      if (this.cantonField.value) {
        this.sectorField.enable();
      } else {
        this.sectorField.setValue("");
        this.sectorField.disable();
      }
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

  get lastTimeSeenField() {
    return this.form.get("lastTimeSeen");
  }

  get lastTimeSeenFieldValid() {
    return this.lastTimeSeenField.touched && this.lastTimeSeenField.valid;
  }

  get lastTimeSeenFieldInvalid() {
    return this.lastTimeSeenField.touched && this.lastTimeSeenField.invalid;
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
}

