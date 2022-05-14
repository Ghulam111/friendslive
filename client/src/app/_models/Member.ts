import { Photo } from "./Photo";

export interface Member {
    id:           number;
    userName:     string;
    photoUrl:     string;
    knownAs:      string;
    age:          number;
    lastActive:   Date;
    createdon:    Date;
    gender:       string;
    interests:    string;
    introduction: string;
    lookingFor:   string;
    city:         string;
    country:      string;
    photos:       Photo[];
}


