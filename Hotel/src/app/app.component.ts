import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, FormsModule],
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'Hotel';

  selectedRoom: number = 1;
  dni: number = 1;
  goscie: number = 1;
  jedzenie: boolean = false;
  koszt: number = 0;

  roomPrices: { [key: number]: number } = {
    1: 80,
    2: 120,
    3: 150
  };

  obliczKoszt() {
    if (this.selectedRoom !== null) {
      let cena = this.roomPrices[this.selectedRoom] * this.dni * this.goscie;
      if (this.jedzenie) {
        cena += 75 * this.dni * this.goscie;
      }
      this.koszt = cena;
    }
  }
}
