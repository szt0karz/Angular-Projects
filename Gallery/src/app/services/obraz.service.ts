import { Injectable } from '@angular/core';
import { obraz } from '../data/obraz-data';
import { Obraz } from '../interfaces/obraz';
import { Subject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ObrazService {

  constructor() { }

  getAllObraz() {return obraz;}
  
  getObrazById(id:number):Obraz
  {
    return obraz[id];
  }

 private subject = new Subject<number>();
 public observable$ = this.subject.asObservable();

 sendId(id:number)
 {
  this.subject.next(id);
 }

 private num = 0;


 nastepnyImg()
 {
   this.num = (this.num+1)% obraz.length;
   this.sendId(this.num);
 }

 
 poprzedniImg()
 {
   this.num = (this.num-1)% obraz.length;
   this.sendId(this.num);
 }

 

}
