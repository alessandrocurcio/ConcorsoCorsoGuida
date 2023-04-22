import { Component, ElementRef, ViewChild } from '@angular/core';
import { Registration } from '../_models/registration';
import { RegistrationService } from '../_services';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent {
  started: boolean = false;
  playing: boolean = false;
  ended: boolean = false;
  name: string = "";
  surname: string = "";
  email: string = "";
  phone: string = "";
  birthDate: Date = new Date();
  readPrivacy: boolean = false;
  finished: boolean = false;
  @ViewChild('video') video: ElementRef | undefined;

  constructor(
    private registrationService: RegistrationService
  )
  {
  }

  start(): void {
    this.started = true;
    this.ended = false;
  }

  save(): void {
    if (!this.validation()) return;

    let r: Registration = {
      name: this.name,
      surname: this.surname,
      email: this.email,
      phone: this.phone,
      birthDate: this.birthDate,
      readPrivacy: this.readPrivacy
    };

    this.registrationService.save(r)
      .subscribe(
        data => {
          this.finished = true;
          this.name = "";
          this.surname = "";
          this.email = "";
          this.phone = "";
          this.birthDate = new Date();
          this.readPrivacy = false;
        },
        error => {
        });
  }

  validation(): boolean {
    if (!this.name) {
      alert("Il campo nome è obbligatorio");
      return false;
    }
    if (!this.surname) {
      alert("Il campo cognome è obbligatorio");
      return false;
    }
    if (!this.email) {
      alert("Il campo email è obbligatorio");
      return false;
    }
    if (!this.phone) {
      alert("Il campo telefono è obbligatorio");
      return false;
    }
    return true;
  }

  videoEnded(event: any): void {
    this.ended = true;
    this.playing = false;
  }

  startAgain(): void {
    this.started = false;
    this.ended = false;
    this.finished = false;
  }

  playVideo(): void {
    this.playing = true;
    this.video!.nativeElement.play();
  }
  pauseVideo(): void {
    this.playing = false;
    this.video!.nativeElement.pause();
  }
}
