import { Data } from "@angular/router";

export class TrafficLight {
  constructor(
    public id?: number,
    public color?: string,
    public date?: Data) { }
}
