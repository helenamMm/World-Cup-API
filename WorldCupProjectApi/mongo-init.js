db = db.getSiblingDB('WorldCupDB');

// Collections
db.createCollection("usuarios", {
    validator: {
        $jsonSchema: {
            bsonType: "object",
            required: ["nombre", "apellido", "correo", "contra", "rol"],
            properties: {
                nombre: { bsonType: "string" },
                apellido: { bsonType: "string" },
                fecha_nacimiento: { bsonType: "date" },
                correo: { bsonType: "string" },
                contra: { bsonType: "string" },
                rol: {
                    bsonType: "string",
                    enum: ["admin", "user"]
                },
                fecha_registro: { bsonType: "date" },
                activo: { bsonType: "bool" },
                favoritos: {
                    bsonType: "object",
                    properties: {
                        partidos: {
                            bsonType: "array",
                            items: { bsonType: "objectId" }
                        },
                        equipos: {
                            bsonType: "array",
                            items: { bsonType: "objectId" }
                        }
                    }
                }
            }
        }
    }
});

db.createCollection("equipos", {
    validator: {
        $jsonSchema: {
            bsonType: "object",
            required: ["nombre", "siglas_equipo", "grupo"],
            properties: {
                nombre: { bsonType: "string" },
                nombre_completo_pais: { bsonType: "string" },
                bandera: { bsonType: "string" },
                informacion: { bsonType: "string" },
                siglas_equipo: { bsonType: "string" },
                grupo: {
                    bsonType: "string",
                    enum: ["A", "B", "C", "D", "E", "F", "G", "H"]
                },
                ranking_fifa: { bsonType: "number" },
                fecha_creacion: { bsonType: "date" },
                jugadores: {
                    bsonType: "array",
                    items: {
                        bsonType: "object",
                        properties: {
                            nombre: { bsonType: "string" },
                            apellido: { bsonType: "string" },
                            fecha_nacimiento: { bsonType: "date" },
                            numero_camiseta: { bsonType: "number" },
                            posicion: { bsonType: "string" }
                        }
                    }
                }
            }
        }
    }
});

db.createCollection("partidos", {
    validator: {
        $jsonSchema: {
            bsonType: "object",
            required: ["equipo_a", "equipo_b", "fecha", "estadio", "estado", "fase"],
            properties: {
                equipo_a: { bsonType: "objectId" },
                equipo_b: { bsonType: "objectId" },
                goles_equipo_a: { bsonType: "number" },
                goles_equipo_b: { bsonType: "number" },
                fecha: { bsonType: "date" },
                estadio: { bsonType: "string" },
                ciudad: { bsonType: "string" },
                estado: {
                    bsonType: "string",
                    enum: ["PROGRAMADO", "EN_JUEGO", "FINALIZADO", "SUSPENDIDO"]
                },
                fase: {
                    bsonType: "string",
                    enum: ["FASE_GRUPOS", "OCTAVOS", "CUARTOS", "SEMIFINAL", "FINAL"]
                },
                grupo: {
                    bsonType: "string",
                    enum: ["A", "B", "C", "D", "E", "F", "G", "H"]
                },
                arbitro_principal: { bsonType: "string" },
                fecha_creacion: { bsonType: "date" },
                fecha_actualizacion: { bsonType: "date" }
            }
        }
    }
});



db.equipos.insertMany([
    {
        _id: ObjectId(),
        nombre: "Argentina",
        nombre_completo_pais: "República Argentina",
        bandera: "https://flagcdn.com/ar.svg",
        siglas_equipo: "ARG",
        grupo: "D",
        ranking_fifa: 1,
        fecha_creacion: new Date(),
        jugadores: [
            {
                _id: ObjectId(),
                nombre: "Lionel",
                apellido: "Messi",
                fecha_nacimiento: new Date("1987-06-24"),
                numero_camiseta: 10,
                posicion: "delantero"
            }
        ]
    },
    {
        _id: ObjectId(),
        nombre: "Brasil",
        nombre_completo_pais: "República Federativa de Brasil",
        bandera: "https://flagcdn.com/br.svg",
        siglas_equipo: "BRA",
        grupo:"F",
        ranking_fifa: 3,
        fecha_creacion: new Date(),
        jugadores: []
    }
]);



print("MongoDB WorldCup database initialized with schema validation and sample data");