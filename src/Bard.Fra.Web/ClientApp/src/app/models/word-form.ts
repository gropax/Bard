
export class WordForm {
  constructor(
    public id: number,
    public graphemes: string,
    public syllables: string,
    public pos: string,
    public number: string,
    public gender: string,
    public person: string,
    public mood: string,
    public tense: string) {
  }
}
