import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Registration } from '../_models';
import { RegistrationService } from '../_services';
import { saveAs } from 'file-saver';


@Component({
  selector: 'app-registration',
  templateUrl: './registration.component.html'
})
export class RegistrationComponent {
  public registrations: Registration[] = [];

  nRow: number = 0;

  constructor(private registrationService: RegistrationService) {
    this.registrationService.getAll().subscribe(
      (r: Registration[]) =>
      {
        this.registrations = r;
      });    
  }

  export(): void {
    this.registrationService.export().subscribe(data => {
      saveAs(data, "Registrazioni.xlsx");
    },
      err => {
        alert("Problem while downloading the file.");
        console.error(err);
      }
    );
  }

  exportRnd(): void {
    this.registrationService.exportRnd(this.nRow, this.registrations.length).subscribe(data => {
      saveAs(data, "Estrazione.xlsx");
    },
      err => {
        alert("Problem while downloading the file.");
        console.error(err);
      }
    );
  }
}
