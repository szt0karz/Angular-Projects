import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { Obraz } from './interfaces/obraz';
import { ObrazService } from './services/obraz.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet,CommonModule],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {

  obraz:Obraz | null = null;

  constructor(public service:ObrazService)
  {
    this.obraz = this.service.getObrazById(0);
        

    service.observable$.subscribe(id => this.obraz = this.service.getObrazById(id-1));
  }

  title = 'Gallery';

  dodajStars(stars:number):string[]
  {
    const maxStars =3;
    const fullStar = "images/stars.png";
    const emptyStar = "images/star_empty.png";
    return Array(stars).fill(fullStar).fill(emptyStar);
  }

  nastepnyImage()
  {
    this.service.nastepnyImg();
  }
 
  
  poprzedniImage()
  {
    this.service.poprzedniImg();
  }
 

}
