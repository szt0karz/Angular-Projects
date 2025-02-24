// app.component.ts
import { Component, signal, effect } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClient, HttpClientModule } from '@angular/common/http';

interface NasaImage {
  date: string;
  explanation: string;
  url: string;
  title: string;
  hdurl?: string;
}

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CommonModule, HttpClientModule],
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  currentDate = signal(new Date());
  selectedImage = signal<NasaImage | null>(null);
  loading = signal(false);
  error = signal('');
  today = new Date();
  darkMode = signal(true);
  favorites = signal<Set<string>>(new Set());

  constructor(private http: HttpClient) {
    this.today.setHours(0, 0, 0, 0);
    effect(() => {
      document.body.classList.toggle('dark-mode', this.darkMode());
    });
    this.loadFavorites();
  }

  // Wczytuje zapisane ulubione obrazy z localStorage
  private loadFavorites() {
    const saved = localStorage.getItem('nasaFavorites');
    if (saved) this.favorites.set(new Set(JSON.parse(saved)));
  }

  // Zwraca tablicę dni w aktualnym miesiącu
  get daysInMonth(): Date[] {
    const date = new Date(this.currentDate());
    date.setDate(1);
    const days = [];
    
    while (date.getMonth() === this.currentDate().getMonth()) {
      days.push(new Date(date));
      date.setDate(date.getDate() + 1);
    }
    return days;
  }

  // Pobiera obraz NASA dla wybranej daty
  async selectDate(date: Date) {
    if (date > this.today || this.loading()) return;

    this.loading.set(true);
    this.error.set('');

    try {
      const image = await this.http.get<NasaImage>(
        `https://api.nasa.gov/planetary/apod?api_key=sVmGR2WpkhOhuodPel7wA9aLVsMiEdqTysMr1w0A&date=${date.toISOString().split('T')[0]}`
      ).toPromise();

      this.selectedImage.set(image || null);
    } catch (err) {
      this.error.set('Nie znaleziono obrazu dla tego dnia');
    } finally {
      this.loading.set(false);
    }
  }

  // Zmienia miesiąc w kalendarzu
  changeMonth(offset: number) {
    this.currentDate.set(new Date(
      this.currentDate().getFullYear(),
      this.currentDate().getMonth() + offset,
      1
    ));
  }

  // Dodaje lub usuwa datę z ulubionych
  toggleFavorite(date: string) {
    const newFavorites = new Set(this.favorites());
    newFavorites.has(date) ? newFavorites.delete(date) : newFavorites.add(date);
    this.favorites.set(newFavorites);
    localStorage.setItem('nasaFavorites', JSON.stringify([...newFavorites]));
  }

  // Przenosi użytkownika do dzisiejszej daty i ładuje jej obraz
  goToToday() {
    const today = new Date();
    this.currentDate.set(today);
    this.selectDate(today);
  }
}