import { AfterViewInit, Component, Input, OnInit } from "@angular/core";
import { FormBuilder, FormGroup } from "@angular/forms";
import { postpetView } from "src/app/models/postpet.model";
import {
  breed,
  canton,
  provincia,
  sector,
} from "../../../models/category.model";
import { switchMap } from "rxjs/operators";
import { CategoryService } from "src/app/services/category.service";
import { PostpetService } from "src/app/services/postpet.service";
import { Observable, Subject } from "rxjs";
import { MyValidators } from "src/app/validators/validators";
import { NoScrollService } from "../../../services/no-scroll.service";
import { ActivatedRoute, ParamMap } from "@angular/router";
import { LoadingService } from "src/app/services/loading.service";

@Component({
  selector: 'app-filter',
  templateUrl: './filter.component.html',
  styleUrls: ['./filter.component.scss']
})
export class FilterComponent implements OnInit {
  constructor(
    private postpetService: PostpetService,
    private formBuilder: FormBuilder,
    private categoryService: CategoryService,
    private noScrollService: NoScrollService,
    private route: ActivatedRoute,
    private loadingService: LoadingService
  ) {
    this.buildForm();
  }


  @Input() stateId: string = "";


  petSpecieId: number | null = null;
  petBreedId: number | null = null;
  petAge: number | null = null;
  provinciaId: number | null = null;
  cantonId: number | null = null;
  sectorId: number | null = null;
  date: string | null = null;
  order: number | null = null;
  secondTime: number = 0;

  breeds: breed[] = [];
  provincias: provincia[] = [];
  cantones: canton[] = [];
  sectores: sector[] = [];

  form: FormGroup;
  postspet: postpetView[] = [];
  limit: number = 10;
  offset: number = 0;
  filter: boolean = false;

  morePostspet: boolean = true;
  loadingSubscription
  isLoadingMore: boolean = false;
  private _isLoading: boolean = false;
  get isLoading(){
    return this._isLoading
  }
  set isLoading(value: boolean){
    this._isLoading = value
    if(!value){
      console.log("unsubscribe")
      this.loadingSubscription.unsubscribe()
      console.log("unsubscribe")
    }
  }
  postspetLoading = [null, null, null, null, null, null, null, null]

  ngOnInit(): void {
    this.loadingSubscription = this.loadingService.isLoading$.subscribe(data => this.isLoading = data)
    this.postpetService.getByState(this.stateId, 10, 0).subscribe((data) => {
      this.offset += this.limit;
      this.postspet = data;
      if(data.length < 10 || data.length > 10){
        this.morePostspet = false;
      }
    });
    this.getBreedsBySpecie();
    this.getProvincias();
    this.getCantonesByProv();
    this.getSectoresByCanton();
    this.toggleDisabledBreed();
    this.toggleDisabledCanton();
    this.toggleDisabledSector();

    this.route.queryParamMap.subscribe((params: ParamMap) => {
      this.petSpecieId = null;
      this.petBreedId = null;
      this.provinciaId = null;
      this.cantonId = null;
      this.sectorId = null;
      this.date = null;
      this.order = null;
      this.secondTime = this.secondTime + 1;

      if (this.form.valid && this.secondTime >= 2) {
        if (parseInt(params.get("especie"))) {
          this.petSpecieId = parseInt(params.get("especie"));
        }
        if (parseInt(params.get("raza"))) {
          this.petBreedId = parseInt(params.get("raza"));
        }
        if (parseInt(params.get("provincia"))) {
          this.provinciaId = parseInt(params.get("provincia"));
        }
        if (parseInt(params.get("canton"))) {
          this.cantonId = parseInt(params.get("canton"));
        }
        if (parseInt(params.get("sector"))) {
          this.sectorId = parseInt(params.get("sector"));
        }
        if (parseInt(params.get("fecha"))) {
          this.date = params.get("fecha");
        }
        if (parseInt(params.get("orden"))) {
          this.order = parseInt(params.get("orden"));
        }
        this.limit = 10;
        this.offset = 0;
        this.postpetService
          .GetByFilter(
            this.stateId,
            this.petSpecieId,
            this.petBreedId,
            this.provinciaId,
            this.cantonId,
            this.sectorId,
            this.date,
            this.order,
            this.limit,
            this.offset
          )
          .subscribe((data) => {
            if (data) {
              this.postspet = data;
              if (data.length < 10) {
                this.morePostspet = false;
              } else {
                this.morePostspet = true;
              }
              this.filter = false;
              this.overflowHidden();
              this.offset += this.limit;
            }
          });
      } else {
        this.form.markAllAsTouched();
      }
    });
  }


  overflowHidden() {
    if (this.filter) {
      this.noScrollService.noScrollEmit(true);
    } else {
      this.noScrollService.noScrollEmit(false);
    }
  }

  onLoadMore() {
    this.isLoadingMore = true;
    this.postpetService
      .GetByFilter(
        this.stateId,
        this.petSpecieId,
        this.petBreedId,
        this.provinciaId,
        this.cantonId,
        this.sectorId,
        this.date,
        this.order,
        this.limit,
        this.offset
      )
      .subscribe((data) => {
        if (data) {
          this.isLoadingMore = false;
          console.log(this.isLoadingMore)
          this.postspet = this.postspet.concat(data);
          if (data.length < 10) {
            this.morePostspet = false;
          } else {
            this.morePostspet = true;
          }
          this.offset += this.limit;
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
      order: ["0"]
    });
  }


  cleanForm() {
    this.petSpecieField.setValue("");
    this.petBreedField.setValue("");
    this.provinciaField.setValue("");
    this.cantonField.setValue("");
    this.sectorField.setValue("");
    this.lastTimeSeenField.setValue("");
    this.orderField.setValue("0");
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

  get orderField() {
    return this.form.get("order");
  }

  private getBreedsBySpecie() {
    this.petSpecieField.valueChanges
      .pipe(
        switchMap((id) => {
          if (id) {
            return this.categoryService.getBreedsBySpecie(id);
          } else {
            return new Observable<null>();
          }
        })
      )
      .subscribe((breeds: breed[] | null) => (this.breeds = breeds));
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
          if (id) {
            return this.categoryService.getCantonesByProv(id);
          } else {
            return new Observable<null>();
          }
        })
      )
      .subscribe((cantones: canton[] | null) => (this.cantones = cantones));
  }

  private getSectoresByCanton() {
    this.cantonField.valueChanges
      .pipe(
        switchMap((id) => {
          if (id) {
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
