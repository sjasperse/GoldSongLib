import { useEffect, useState } from "react";
import { useDrag } from "react-dnd";
import Loading from "../Loading";
import { getSongs } from "../services/apiClient";
import { Song } from "../types";
import { SongDropResult } from "./WorshipOrderEditorForm";

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
        {songs.map(song => <SongReference key={song.id} song={song} />)}
      </ul>
    }
  </div>
}

function SongReference({ song }: { song: Song }) {
  const [_, dragRef] = useDrag({
    type: "Song",
    item: song,
    collect: monitor => {},
    end: (_, monitor) => {
      const dropResult = monitor.getDropResult<SongDropResult>();
      dropResult?.onDropped(song);
    }
  });

  return <li ref={dragRef}>{song.name}</li>
}
