namespace GameOfLife

open System

module Grid =

  let neighbours (x, y) =
    let coordinates = [| -1; 0; 1 |]

    Array.allPairs coordinates coordinates
    |> Array.map (fun (x', y') -> x + x', y + y')
    |> Array.filter ((<>) (x, y))

  let aliveNeighbours xy grid = neighbours xy |> Array.filter (fun xy -> grid |> Array.contains xy)

  let survivors grid =
    grid
    |> Array.filter (fun xy ->
      let n = grid |> aliveNeighbours xy |> Array.length
      n = 2 || n = 3)

  let births grid =
    grid
    |> Array.collect neighbours
    |> Array.filter (fun xy -> grid |> aliveNeighbours xy |> Array.length = 3)

  let tick grid =
    let next = survivors grid |> Array.append (births grid)
    next |> Array.distinct

  let random width height =
    let r = Random()

    [| 0..width |]
    |> Array.collect (fun x -> [| 0..height |] |> Array.map (fun y -> x, y))
    |> Array.filter (fun _ -> r.Next(0, 5) > 3)
