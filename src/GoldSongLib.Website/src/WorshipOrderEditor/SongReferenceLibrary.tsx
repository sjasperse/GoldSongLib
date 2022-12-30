import { useEffect, useState } from "react";
import Loading from "../Loading";
import { getSongs } from "../services/apiClient";
import { Song } from "../types";

export default function SongReferenceLibrary() {
  const [songs, setSongs] = useState<Song[]>();

  useEffect(() => {
    getSongs().then(setSongs);
  }, []);

  return <div>
    <h2>Songs</h2>
    {songs == undefined 
      ? <Loading />
      : <ul>
        {songs.map(song => <li key={song.id}>{song.name}</li>)}
      </ul>
    }
  </div>
}
